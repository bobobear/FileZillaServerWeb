﻿using FileZillaServerDAL;
using FileZillaServerModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FileZillaServerBLL
{
    public class FileHistoryBLL
    {
        private readonly FileHistoryDAL dal = new FileHistoryDAL();
        public FileHistoryBLL()
        { }
        #region  BasicMethod
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string ID)
        {
            return dal.Exists(ID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(FileHistory model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(FileHistory model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string ID)
        {

            return dal.Delete(ID);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string IDlist)
        {
            return dal.DeleteList(IDlist);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public FileHistory GetModel(string ID)
        {

            return dal.GetModel(ID);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere, string orderBy)
        {
            return dal.GetList(strWhere, orderBy);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<FileHistory> GetModelList(string strWhere, string orderBy)
        {
            DataSet ds = dal.GetList(strWhere, orderBy);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<FileHistory> DataTableToList(DataTable dt)
        {
            List<FileHistory> modelList = new List<FileHistory>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                FileHistory model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = dal.DataRowToModel(dt.Rows[n]);
                    if (model != null)
                    {
                        modelList.Add(model);
                    }
                }
            }
            return modelList;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("", "");
        }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            return dal.GetRecordCount(strWhere);
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            return dal.GetListByPage(strWhere, orderby, startIndex, endIndex);
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        //public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        //{
        //return dal.GetList(PageSize,PageIndex,strWhere);
        //}

        #endregion  BasicMethod
        #region  ExtensionMethod
        public string GetTaskNoById(string fileHistoryId)
        {
            return dal.GetTaskNoById(fileHistoryId);
        }

        public string GetProjectIdById(string fileHistoryId)
        {
            return dal.GetProjectIdById(fileHistoryId);
        }

        /// <summary>
        /// 判断是否已经设置了完成人，即是否已经分配
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public DataSet GetTaskNoAndEmpNoByPrjId(string projectID)
        {
            return dal.GetTaskNoAndEmpNoByPrjId(projectID);
        }
        public List<FileHistory> GetFileHistories(HttpContext context, out int errCode)
        {
            errCode = 0;
            string projectId = context.Request["projectId"];
            string where = string.Format(" AND PARENTID IN ( SELECT ID FROM filecategory WHERE projectId = '{0}') ", projectId);
            // 加入排序字段
            string orderBy = string.Format(" ORDER BY operateDate");
            List<FileHistory> fileHistories = this.GetModelList(where, orderBy);
            return fileHistories;
        }

        /// <summary>
        /// 添加一条文件记录
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="fileName">文件名</param>
        /// <param name="fileNameRelativeToTaskRoot">相对于任务目录的文件名（从任务编号的目录之后开始取）</param>
        /// <param name="description"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool AddFileHistory(string parentId, string fileName, string fileNameRelativeToTaskRoot, string description, string userId)
        {
            FileHistory fileHistory = new FileHistory();
            fileHistory.ID = Guid.NewGuid().ToString();
            fileHistory.PARENTID = parentId;
            fileHistory.FILENAME = fileName;
            fileHistory.FILEFULLNAME = fileNameRelativeToTaskRoot;
            fileHistory.FILEEXTENSION = Path.GetExtension(fileName);
            fileHistory.DESCRIPTION = description;
            fileHistory.OPERATEDATE = DateTime.Now;
            fileHistory.OPERATEUSER = userId;
            fileHistory.ISDELETED = false;
            bool addFileHisFlag = this.Add(fileHistory);
            return addFileHisFlag;
        }
        #endregion  ExtensionMethod
    }
}
