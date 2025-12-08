using System;
using JollyArt;


[Serializable]
public class TaskItemData
{
    public int taskId;
    public string amount;
    public int status;  //1可领取 1已经领取 其他未领取
    public int taskTag;  //任务标签 共有1西瓜 2转盘 3定时红包 4道具 5视频
    public int tagNum;  //完成任务需要的对应任务类型的数量 比如需要合成2个西瓜
    public int alreadyNum;  // 0可领取 1已经领取 其他未领取
    public int alreadyAllNum;  //已完成的对应任务的总数 例如 使用2次道具 后台无需关注
}

[Serializable]
public class TaskListData
{
    public TaskItemData[] taskList;
    public TaskItemData[] achievementList;
}


[Serializable]
public class TaskData : JsonDataBase
{
    public TaskListData data;

    public static TaskData Parse(string json)
    {
        return JsonParser.ToObject<TaskData>(json);
    }
}

[Serializable]
public class TaskRewardData : JsonDataBase
{
    public string amount;

    public static TaskRewardData Parse(string json)
    {
        return JsonParser.ToObject<TaskRewardData>(json);
    }
}
    
