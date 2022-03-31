```csharp
namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription(@"对集合进行迭代访问，将当前索引设置给CurrentIndex，当前元素设置给CurrentElement")]
    [TaskIcon("{SkinColor}RepeaterIcon.png")]
    public class For : Decorator
    {
        [Tooltip("Should the task return if the child task returns a failure")]
        public SharedBool endOnFailure;
        [Tooltip("集合")]
        public SharedVariable collection;
        [Tooltip("当前访问索引")]
        public SharedInt currentIndex;
        [Tooltip("当前访问元素")] 
        public SharedVariable currentElement;

        // The number of times the child task has been run.
        private int executionCount = 0;
        // The status of the child after it has finished running.
        private TaskStatus executionStatus = TaskStatus.Inactive;

        public override bool CanExecute()
        {
            var elements = (collection.GetValue() as System.Collections.IList);
            // Continue executing until we've reached the count or the child task returned failure and we should stop on a failure.
            return (executionCount < elements.Count) && (!endOnFailure.Value || (endOnFailure.Value && executionStatus != TaskStatus.Failure));
        }

        public override void OnChildExecuted(TaskStatus childStatus)
        {
            // The child task has finished execution. Increase the execution count and update the execution status.
            executionCount++;
            executionStatus = childStatus;

            var elements = collection.GetValue() as System.Collections.IList;
            if (elements == null || executionCount >= elements.Count) return;

            currentIndex.SetValue(executionCount);
            currentElement.SetValue(elements[executionCount]);
        }

        public override void OnEnd()
        {
            // Reset the variables back to their starting values.
            executionCount = 0;
            executionStatus = TaskStatus.Inactive;
        }

        public override void OnReset()
        {
            endOnFailure = true;
        }

        public override void OnStart()
        {
            var elements = collection.GetValue() as System.Collections.IList;
            if (elements == null) return;
            currentIndex.SetValue(executionCount);
            currentElement.SetValue(elements[executionCount]);
        }
    }
}
```

