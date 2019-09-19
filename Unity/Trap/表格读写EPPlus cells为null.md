### 报空错误
``` csharp
using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
{
    // cells永远是null，但是其字段Value里却包含有表格的信息
    object[,] cells = excelPackage.Workbook.Worksheets[1].Cells;
}
```

### 解决方案
``` csharp
using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
{
    var sheet = excelPackage.Workbook.Worksheets[1];
    object[,] cells = sheet.Cells.Value as object[,];
}
```
