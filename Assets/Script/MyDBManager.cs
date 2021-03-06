using UnityEngine;
using Mono.Data.Sqlite;
using System.IO;
using System;
using System.Collections.Generic;

public class MyDBManager
{

    private string dbFilePath = Application.dataPath + "/../FanRenData/originData.db";
    private SqliteConnection mSqliteConnection;
    private static MyDBManager mMyDBManager = new MyDBManager();

    private bool mIsConnected = false;

    private MyDBManager()
    {
    }

    public static MyDBManager GetInstance()
    {
        return mMyDBManager;
    }

    public bool ConnDB()
    {
        if (this.mIsConnected) return true;
        try
        {
            if (!Directory.Exists(new FileInfo(dbFilePath).Directory.FullName))
            {
                Directory.CreateDirectory(new FileInfo(dbFilePath).Directory.FullName);
            }
            if (!File.Exists(dbFilePath))
            {
                SqliteConnection.CreateFile(dbFilePath);
            }
            if (mSqliteConnection == null)
            {
                mSqliteConnection = new SqliteConnection(new SqliteConnectionStringBuilder() { DataSource = dbFilePath }.ToString());
            }
            mSqliteConnection.Open();
            this.mIsConnected = true;
            return this.mIsConnected;
        }
        catch (Exception e)
        {
            Debug.LogError("ConnDB error : " + e.ToString());
            this.mIsConnected = false;
            return this.mIsConnected;
        }
    }

    public bool IsConnected()
    {
        return this.mIsConnected;
    }

    public RoleInfo GetRoleInfo(int roleId)
    {
        RoleInfo roleInfo = new RoleInfo();
        SqliteCommand sqliteCommand = this.mSqliteConnection.CreateCommand();
        sqliteCommand.CommandText = $"select * from role_info_r where roleId={roleId}";
        SqliteDataReader sdr = sqliteCommand.ExecuteReader();
        if (sdr.Read())
        {
            //todo
        }
        sdr.Close();
        sdr.Dispose();
        sqliteCommand.Dispose();
        return roleInfo;
    }

    public List<Shentong> GetRoleActiveShentong(int roleId)
    {
        List<Shentong> roleShentong = new List<Shentong>();
        SqliteCommand sqliteCommand = this.mSqliteConnection.CreateCommand();
        sqliteCommand.CommandText = $"select * from role_active_shentong_rw where roleId={roleId}";
        SqliteDataReader sdr = sqliteCommand.ExecuteReader();
        while (sdr.Read())
        {
            //todo
        }
        sdr.Close();
        sdr.Dispose();
        sqliteCommand.Dispose();
        return roleShentong;
    }

    //????????
    public class RoleInfo
    {
        public int roleId;
        public string name;
        public int currentHp;
        public int maxHp;
        public int currentMp;
        public int maxMp;
    }

    //??????????????
    public class RoleItem{
        public int itemId;
        public int itemCount;
        public int itemType;
    }

    public RoleItem GetRoleItem(int itemId)
    {
        RoleItem roleItem = new RoleItem();
        SqliteCommand sqliteCommand = this.mSqliteConnection.CreateCommand();
        sqliteCommand.CommandText = $"select * from role_bag_rw where itemId={itemId}";
        SqliteDataReader sdr = sqliteCommand.ExecuteReader();
        if (sdr.Read())
        {
            roleItem.itemId = itemId;
            roleItem.itemCount = (int)((Int64)sdr["itemCount"]);
            roleItem.itemType = (int)((Int64)sdr["itemType"]);
        }
        sdr.Close();
        sdr.Dispose();
        sqliteCommand.Dispose();
        return roleItem;
    }


    public bool AddRoleTask(int taskId)
    {
        SqliteCommand sqliteCommand = this.mSqliteConnection.CreateCommand();
        sqliteCommand.CommandText = $"select * from role_tasks_rw where taskId={taskId}";
        SqliteDataReader sdr = sqliteCommand.ExecuteReader();
        if (sdr.Read())
        {
            //????????????
            Debug.Log("???????????? taskId " + taskId);
            sdr.Close();
            sdr.Dispose();
            sqliteCommand.Dispose();

            return true;
        }
        else
        {
            sdr.Close();
            sdr.Dispose();
            sqliteCommand.Dispose();

            SqliteCommand sqliteCommand2 = this.mSqliteConnection.CreateCommand();
            sqliteCommand2.CommandText = $"insert into role_tasks_rw (taskId, taskState) values ({taskId}, {((int)FRTaskState.InProgress)})";
            bool result = sqliteCommand2.ExecuteNonQuery() == 1;
            sqliteCommand2.Dispose();
            return result;
        }
    }

    //????????
    public class RoleTask
    {
        public int taskId;
        public int taskState;
        public string remark;
        public int isMainTask;
        public int storyLineIndex;
        public int triggerRoleId;
        public int submitRoleId;
    }

    //????????????
    public RoleTask GetRoleTask(int taskId)
    {
        Debug.Log("GetRoleTask taskId : " + taskId);
        SqliteCommand sqliteCommand = this.mSqliteConnection.CreateCommand(); 
        sqliteCommand.CommandText = $"select * from tasks_r a left join role_tasks_rw b on a.taskId=b.taskId where a.taskId={taskId}";
        SqliteDataReader sdr = sqliteCommand.ExecuteReader();
        if (sdr.Read())
        {
            RoleTask roleTask = new RoleTask();
            roleTask.taskId = taskId;
            roleTask.taskState = sdr["taskState"].Equals(DBNull.Value) ? (int)FRTaskState.Untrigger : (int)((Int64)sdr["taskState"]);
            roleTask.remark = (string)sdr["remark"];
            roleTask.isMainTask = (int)((Int64)sdr["isMainTask"]);
            roleTask.storyLineIndex = (int)((Int64)sdr["storyLineIndex"]);
            roleTask.triggerRoleId = (int)((Int64)sdr["triggerRoleId"]);
            roleTask.submitRoleId = (int)((Int64)sdr["submitRoleId"]);

            sdr.Close();
            sdr.Dispose();
            sqliteCommand.Dispose();

            return roleTask;
        }
        else
        {
            Debug.LogError("?????????????????? GetRoleTask taskId : " + taskId);
            return null;
        }
    }

    /**
     * ??????????????????????
      **/
    public RoleTask GetTriggedRoleTask(int taskId)
    {
        SqliteCommand sqliteCommand = this.mSqliteConnection.CreateCommand();
        sqliteCommand.CommandText = $"select * from role_tasks_rw a left join tasks_r b on a.taskId=b.taskId where a.taskId={taskId}";
        SqliteDataReader sdr = sqliteCommand.ExecuteReader();
        if (sdr.Read())
        {
            RoleTask roleTask = new RoleTask();
            roleTask.taskId = taskId;
            roleTask.taskState = (int)((Int64)sdr["taskState"]);
            roleTask.remark = (string)sdr["remark"];
            roleTask.isMainTask = (int)((Int64)sdr["isMainTask"]);
            roleTask.storyLineIndex = (int)((Int64)sdr["storyLineIndex"]);
            roleTask.triggerRoleId = (int)((Int64)sdr["triggerRoleId"]);
            roleTask.submitRoleId = (int)((Int64)sdr["submitRoleId"]);

            sdr.Close();
            sdr.Dispose();
            sqliteCommand.Dispose();

            return roleTask;
        }
        else
        {
            return null;
        }
    }

    //????????????????????????????
    public List<RoleTask> GetRoleTasks(int roleId)
    {
        SqliteCommand sqliteCommand = this.mSqliteConnection.CreateCommand();
        sqliteCommand.CommandText = $"select * from tasks_r a left join role_tasks_rw b on a.taskId=b.taskId where a.triggerRoleId={roleId}";
        SqliteDataReader sdr = sqliteCommand.ExecuteReader();
        List<RoleTask> results = new List<RoleTask>();
        while (sdr.Read())
        {
            RoleTask roleTask = new RoleTask();
            roleTask.taskId = (int)((Int64)sdr["taskId"]); 
            roleTask.taskState = sdr["taskState"].Equals(DBNull.Value) ? (int)FRTaskState.Untrigger : ((int)((Int64)sdr["taskState"]));
            roleTask.remark = (string)sdr["remark"];
            roleTask.isMainTask = (int)((Int64)sdr["isMainTask"]);
            roleTask.storyLineIndex = (int)((Int64)sdr["storyLineIndex"]);
            roleTask.triggerRoleId = roleId;
            roleTask.submitRoleId = (int)((Int64)sdr["submitRoleId"]);

            results.Add(roleTask);
        }
        sdr.Close();
        sdr.Dispose();
        sqliteCommand.Dispose();
        return results;
    }

    public bool UpdateRoleTaskState(int taskId, FRTaskState taskState)
    {
        SqliteCommand sqliteCommand = this.mSqliteConnection.CreateCommand();
        sqliteCommand.CommandText = $"update role_tasks_rw set taskState={((int)taskState)} where taskId={taskId}";
        bool result = sqliteCommand.ExecuteNonQuery() == 1;
        sqliteCommand.Dispose();
        return result;
    }

    //????????????????????
    public List<RoleTask> GetAllLeaderActorInProgressTasks()
    {
        SqliteCommand sqliteCommand = this.mSqliteConnection.CreateCommand();
        sqliteCommand.CommandText = $"select * from role_tasks_rw a left join tasks_r b on a.taskId = b.taskId where a.taskState={((int)FRTaskState.InProgress)}";
        SqliteDataReader sdr = sqliteCommand.ExecuteReader();

        List<RoleTask> roleTasks = new List<RoleTask>();
        while (sdr.Read())
        {
            RoleTask roleTask = new RoleTask();
            roleTask.taskId = (int)((Int64)sdr["taskId"]);
            roleTask.taskState = (int)((Int64)sdr["taskState"]);
            roleTask.remark = (string)sdr["remark"];
            roleTask.isMainTask = (int)((Int64)sdr["isMainTask"]);
            roleTask.storyLineIndex = (int)((Int64)sdr["storyLineIndex"]);
            roleTask.triggerRoleId = (int)((Int64)sdr["triggerRoleId"]);
            roleTask.submitRoleId = (int)((Int64)sdr["submitRoleId"]);

            roleTasks.Add(roleTask);
        }
        sdr.Close();
        sdr.Dispose();
        sqliteCommand.Dispose();
        return roleTasks;
    }

    //??????NPC????????????????
    public List<RoleTask> GetAllLeaderActorWithNPCTriggerTasks(int triggerNPCRoleId)
    {
        SqliteCommand sqliteCommand = this.mSqliteConnection.CreateCommand();
        //sqliteCommand.CommandText = $"select * from role_tasks_rw a left join tasks_r b on a.taskId = b.taskId where a.taskState={((int)state)} and b.triggerRoleId={triggerNPCRoleId}";
        sqliteCommand.CommandText = $"select * from tasks_r a left join role_tasks_rw b on a.taskId = b.taskId where a.triggerRoleId={triggerNPCRoleId}";
        SqliteDataReader sdr = sqliteCommand.ExecuteReader();
        List<RoleTask> roleTasks = new List<RoleTask>();
        while (sdr.Read())
        {
            RoleTask roleTask = new RoleTask();
            roleTask.taskId = (int)((Int64)sdr["taskId"]);
            roleTask.taskState = sdr["taskState"].Equals(DBNull.Value) ? (int)FRTaskState.Untrigger : ((int)((Int64)sdr["taskState"]));
            roleTask.remark = (string)sdr["remark"];
            roleTask.isMainTask = (int)((Int64)sdr["isMainTask"]);
            roleTask.storyLineIndex = (int)((Int64)sdr["storyLineIndex"]);
            roleTask.triggerRoleId = triggerNPCRoleId;
            roleTask.submitRoleId = (int)((Int64)sdr["submitRoleId"]);

            roleTasks.Add(roleTask);
        }
        sdr.Close();
        sdr.Dispose();
        sqliteCommand.Dispose();
        return roleTasks;
    }

    //??????NPC????????????????
    public List<RoleTask> GetAllLeaderActorWithNPCSubmitTasks(int submitNPCRoleId)
    {
        SqliteCommand sqliteCommand = this.mSqliteConnection.CreateCommand();
        //sqliteCommand.CommandText = $"select * from role_tasks_rw a left join tasks_r b on a.taskId = b.taskId where a.taskState={((int)state)} and b.triggerRoleId={triggerNPCRoleId}";
        sqliteCommand.CommandText = $"select * from tasks_r a left join role_tasks_rw b on a.taskId = b.taskId where a.submitRoleId={submitNPCRoleId}";
        SqliteDataReader sdr = sqliteCommand.ExecuteReader();
        List<RoleTask> roleTasks = new List<RoleTask>();
        while (sdr.Read())
        {
            RoleTask roleTask = new RoleTask();
            roleTask.taskId = (int)((Int64)sdr["taskId"]);
            roleTask.taskState = sdr["taskState"].Equals(DBNull.Value) ? (int)FRTaskState.Untrigger : ((int)((Int64)sdr["taskState"]));
            roleTask.remark = (string)sdr["remark"];
            roleTask.isMainTask = (int)((Int64)sdr["isMainTask"]);
            roleTask.storyLineIndex = (int)((Int64)sdr["storyLineIndex"]);
            roleTask.triggerRoleId = (int)((Int64)sdr["triggerRoleId"]);
            roleTask.submitRoleId = (int)((Int64)sdr["submitRoleId"]);

            roleTasks.Add(roleTask);
        }
        sdr.Close();
        sdr.Dispose();
        sqliteCommand.Dispose();
        return roleTasks;
    }

    public bool AddItemToBag(int itemId, FRItemType itemType, int addCount)
    {
        SqliteCommand sqliteCommand = null;
        SqliteCommand sqliteCommand2 = null;
        SqliteDataReader sdr = null;
        try
        {
            sqliteCommand = this.mSqliteConnection.CreateCommand();
            sqliteCommand.CommandText = $"select * from role_bag_rw where itemId={itemId}";
            sdr = sqliteCommand.ExecuteReader();

            sqliteCommand2 = this.mSqliteConnection.CreateCommand();
            if (sdr.Read())
            {
                Int64 originCount = (Int64)sdr["itemCount"];
                Int64 resultCount = originCount + addCount;
                sqliteCommand2.CommandText = $"update role_bag_rw set itemCount={resultCount} where itemId={itemId}";
            }
            else
            {
                //insert
                sqliteCommand2.CommandText = $"insert into role_bag_rw (itemId, itemCount, itemType) values ({itemId}, {addCount}, {((int)itemType)})";
            }
            bool result = sqliteCommand2.ExecuteNonQuery() == 1;
            return result;
        }
        catch(Exception e)
        {
            Debug.LogError("AddItemToBag, " + e.ToString());
            return false;
        }
        finally
        {
            if(sdr != null)
            {
                sdr.Close();
                sdr.Dispose();
            }
            if(sqliteCommand != null)
            {
                sqliteCommand.Dispose();
            }
            if (sqliteCommand2 != null)
            {
                sqliteCommand2.Dispose();
            }
        }
    }

    public bool DeleteItemInBag(int itemId, int deleteCount, int ownCount)
    {
        SqliteCommand sqliteCommand = null;
        try
        {
            sqliteCommand = this.mSqliteConnection.CreateCommand();
            if(deleteCount == ownCount)
            {
                sqliteCommand.CommandText = $"delete from role_bag_rw where itemId={itemId}";
            }
            else if(deleteCount < ownCount)
            {
                sqliteCommand.CommandText = $"update role_bag_rw set itemCount={ownCount-deleteCount} where itemId={itemId}";
            }
            else
            {
                Debug.LogError("DeleteItemInBag ????????");
                return false;
            }
            return sqliteCommand.ExecuteNonQuery() == 1;
        }
        catch (Exception e)
        {
            Debug.LogError("AddItemToBag, " + e.ToString());
            return false;
        }
        finally
        {
            if (sqliteCommand != null)
            {
                sqliteCommand.Dispose();
            }
        }
    }

}
