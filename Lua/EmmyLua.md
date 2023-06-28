# 调试

- 安装IDEA社区版2021
- File->Setting->Plugins->EmmyLua
- DebugConfigurations里添加配置，选择Emmy Debugger(NEW)，Connection选择```TCP(Debugger connect IDE)```，将下方的编辑器通过TCP连接IDE的lua启动代码复制并粘贴到lua入口处
- 点击debug（虫子）按钮，然后（从lua入口处）运行lua脚本

# Unity中的代码提示
在安装了对应的IDEA插件后，将以下文件EmmyLuaService.cs复制到工程的某个Editor目录下。
``` c#
using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using UnityEditor;
using UnityEditor.Callbacks;

namespace EmmyLua
{
	enum TypeKind
	{
		Class,
		Array,
	}

	enum Proto
	{
		Lib,
		Ping
	}

	[InitializeOnLoad]
	class EmmyLuaService
	{
		private static Socket socket;

		private static Thread receiveThread;

		private static int PORT = 996;

		private static bool doTryLater;
		
		private static DateTime lastTime;

		private static bool connected;

		private static bool running;

		private const string KEY_EMMY_SERVICE_ENABLE = "emmy.service.enable";

		private static bool IsEnable
		{
			get { return EditorPrefs.GetBool(KEY_EMMY_SERVICE_ENABLE); }
			set { EditorPrefs.SetBool(KEY_EMMY_SERVICE_ENABLE, value); }
		}

		[MenuItem("EmmyLua/Enable")]
		static void EnableService()
		{
			IsEnable = true;
			StartConnect();
		}
		
		[MenuItem("EmmyLua/Enable", true)]
		static bool EnableServiceCheck()
		{
			return !IsEnable;
		}
		
		[MenuItem("EmmyLua/Disable")]
		static void DisableService()
		{
			IsEnable = false;
			Stop();
		}
		
		[MenuItem("EmmyLua/Disable", true)]
		static bool DisableServiceCheck()
		{
			return IsEnable;
		}

		static EmmyLuaService()
		{
			EditorApplication.update += Update;
			StartConnect();
		}

		static void StartConnect()
		{
			if (running || !IsEnable)
				return;
			running = true;
			connected = false;
			doTryLater = false;
			try
			{
				if (socket != null)
					socket.Close();
				socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				socket.BeginConnect("127.0.0.1", PORT, OnConnect, socket);
			}
			catch (Exception e)
			{
				TryLater();
			}
		}

		private static void OnConnect(IAsyncResult ar)
		{
			try
			{
				socket.EndConnect(ar);
				connected = true;
				SendData(socket);
			}
			catch (Exception e)
			{
				TryLater();
			}
		}

		private static void TryLater()
		{
			running = false;
			connected = false;
			doTryLater = true;
			lastTime = DateTime.Now;
		}

		private static void Stop()
		{
			if (running)
			{
				running = false;
				connected = false;
				doTryLater = false;
				
				if (socket != null)
					socket.Close();
			}
		}

		private static void Update()
		{
			if (!IsEnable)
				return;
			var sp = DateTime.Now - lastTime;
			if (sp.TotalSeconds > 5)
			{
				if (connected)
				{
					Ping();
				}
				else if (doTryLater)
				{
					StartConnect();
				}
			}
		}

		[DidReloadScripts]
		static void UpdateScripts()
		{
			StartConnect();
		}

		private static void WriteString(BinaryWriter writer, string value)
		{
			var encoding = Encoding.UTF8;
			var chars = encoding.GetBytes(value);
			writer.Write(chars.Length);
			writer.Write(chars);
		}

		private static void WriteType(BinaryWriter write, Type type)
		{
			if (type.IsArray)
			{
				write.Write((byte) TypeKind.Array);
				WriteType(write, type.GetElementType());
			}
			else
			{
				write.Write((byte) TypeKind.Class);
				WriteString(write, type.FullName ?? "any");
			}
		}

		private static void Ping()
		{
			using (var buf = new MemoryStream())
			{
				var writer = new BinaryWriter(buf);
				writer.Write(8);
				writer.Write((int) Proto.Ping);
				try
				{
					var bytes = buf.GetBuffer();
					socket.Send(bytes, 8, SocketFlags.None);
				}
				catch (Exception e)
				{
					TryLater();
				}
			}
		}
		
		private static void SendData(Socket client)
		{
			var buf = new MemoryStream();
			var writer = new BinaryWriter(buf);
			writer.Seek(8, SeekOrigin.Begin);
			var types = GetTypes();
			foreach (var type in types)
			{
				WriteTypeData(type, writer);
			}

			writer.Flush();
			// write size and proto
			var len = (int) buf.Length;
			writer.Seek(0, SeekOrigin.Begin);
			writer.Write(len);
			writer.Write((int) Proto.Lib);
			writer.Flush();
			// send
			client.Send(buf.GetBuffer(), len, SocketFlags.None);
			writer.Close();
		}

		private static void WriteTypeData(Type type, BinaryWriter writer)
		{
			var fullName = type.FullName;
			if (!string.IsNullOrEmpty(fullName))
			{
				// full name
				WriteString(writer, fullName);

				// base type full name
				{
					string baseTypeFullName = null;
					if (type.BaseType != null)
						baseTypeFullName = type.BaseType.FullName;
					writer.Write(baseTypeFullName != null);
					if (baseTypeFullName != null)
						WriteString(writer, baseTypeFullName);
				}

				// fields
				var fields =
					type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
				writer.Write(fields.Length);
				foreach (var fi in fields)
				{
					WriteString(writer, fi.Name);
					WriteType(writer, fi.FieldType);
				}

				// properties
				var properties =
					type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static| BindingFlags.DeclaredOnly);
				writer.Write(properties.Length);
				foreach (var pi in properties)
				{
					WriteString(writer, pi.Name);
					WriteType(writer, pi.PropertyType);
				}

				// methods
				var methods =
					(from mi in type.GetMethods(BindingFlags.Public | BindingFlags.Instance| BindingFlags.Static | BindingFlags.DeclaredOnly)
						where !mi.Name.StartsWith("get_") && !mi.Name.StartsWith("set_")
						select mi).ToArray();

				writer.Write(methods.Count());
				foreach (var mi in methods)
				{
					// name
					WriteString(writer, mi.Name);
					
					// is static
					writer.Write(mi.IsStatic);

					// parameters
					var parameterInfos = mi.GetParameters();
					writer.Write(parameterInfos.Length);
					foreach (var pi in parameterInfos)
					{
						WriteString(writer, pi.Name);
						WriteType(writer, pi.ParameterType);
					}

					// returns
					WriteType(writer, mi.ReturnType);
				}
			}
		}

		private static Type[] GetTypes()
		{
			var unityTypes = from assembly in AppDomain.CurrentDomain.GetAssemblies()
				where !(assembly.ManifestModule is ModuleBuilder)
				      && assembly.FullName != "UnityEditor"
				from type in assembly.GetExportedTypes()
				where type.BaseType != typeof(MulticastDelegate)
				      && !type.IsInterface 
				      && !type.IsEnum
				      //&& !type.IsGenericType
				      //&& !type.IsGenericTypeDefinition
				      && !type.IsNested
				      && !IsExcluded(type)
				select type;
			var arr = unityTypes.ToArray();

			return arr;
		}

		private static bool IsExcluded(Type type)
		{
			return false;
		}
	}
}
```

# 注解

``` lua
-- class注解用于标记哪个是类
---@class ClassA
---@field field1 number
local ClassA = class()
ClassA.field1 = 1

-- type注解用于标记局部/全局变量类型，因为有些变量的类型无法通过函数返回值获得
---@type ClassA
local ClassA = require("Lua.ClassA")

-- generic注解用于表示类型参数的类型（这里的prototype相当于类型参数）
-- param注解用于表示函数各个参数的类型
-- return注解用于表示返回值类型
---@generic TCharacter : ClassA
---@param prototype TCharacter
---@return TCharacter
function Character.Create(prototype)
    return prototype()
end

---具体table类型  {a:number, b:string}
---@type {a:number, b:string}
local t = {a = 1, b = "2"}
```


