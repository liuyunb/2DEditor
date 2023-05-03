using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITimeSystem : ISystem
{
    float CurrentSeconds { get; }

    void AddDelayTask(float seconds, Action OnFinish);
}

public enum DelayTaskState//任务所在状态
{
    NotStart,
    Started,
    Finish
}

public class DelayTask
{
    public float Seconds { get; set; }//任务执行秒数
    public Action OnFinish { get; set; }//完成任务需要执行的回调
    public float StartSeconds { get; set; }//开始时间
    public float FinishSeconds { get; set; }//结束时间
    public DelayTaskState State { get; set; }//任务状态
}

public class TimeSystem : AbstractSystem, ITimeSystem
{
    public class TimeSystemUpdateBehaviour : MonoBehaviour
    {
        public Action OnUpdate;

        private void Update()
        {
            OnUpdate?.Invoke();
        }
    }
    
    protected override void OnInit()
    {
        var updateBehabiourGameobject = new GameObject(nameof(TimeSystemUpdateBehaviour));
        UnityEngine.Object.DontDestroyOnLoad(updateBehabiourGameobject);

        var updateBehaviour = updateBehabiourGameobject.AddComponent<TimeSystemUpdateBehaviour>();
        updateBehaviour.OnUpdate += OnUpdate;
    }
    
    public float CurrentSeconds { get;private set; }

    private LinkedList<DelayTask> _mDelayTasks = new LinkedList<DelayTask>();

    public void OnUpdate()
    {
        CurrentSeconds += Time.deltaTime;

        if (_mDelayTasks.Count > 0)
        {
            var currentNode = _mDelayTasks.First;

            while (currentNode != null)
            {
                var delayTask = currentNode.Value;
                var nextNode = currentNode.Next;

                if (delayTask.State == DelayTaskState.NotStart)
                {
                    delayTask.State = DelayTaskState.Started;
                    delayTask.StartSeconds = CurrentSeconds;
                    delayTask.FinishSeconds = CurrentSeconds + delayTask.Seconds;
                }
                else if (delayTask.State == DelayTaskState.Started)
                {
                    if (CurrentSeconds > delayTask.FinishSeconds)//是否完成任务
                    {
                        delayTask.State = DelayTaskState.Finish;
                        delayTask.OnFinish?.Invoke();
                        delayTask.OnFinish = null;
                        _mDelayTasks.Remove(currentNode);
                    }
                }

                currentNode = nextNode;
            }
        }
    }
    
    public void AddDelayTask(float seconds, Action OnFinish)
    {
        var delayTask = new DelayTask()
        {
            Seconds = seconds,
            OnFinish = OnFinish,
            State = DelayTaskState.NotStart
        };

        _mDelayTasks.AddLast(delayTask);
    }
}
