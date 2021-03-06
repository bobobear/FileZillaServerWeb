﻿using FileZillaServerBLL;
using FileZillaServerCommonHelper;
using FileZillaServerModel;
using FileZillaServerModel.Interface;
using Jil;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Yiliangyijia.Comm;

namespace FileZillaServerWeb
{
    /// <summary>
    /// MyHandler 的摘要说明
    /// </summary>
    public class MyHandler : IHttpHandler
    {
        EmployeeAccountBLL empAcctBll = new EmployeeAccountBLL();
        FileHistoryBLL fileHistoryBll = new FileHistoryBLL();
        FileCategoryBLL fcBll = new FileCategoryBLL();
        ProjectBLL prjBll = new ProjectBLL();

        public void ProcessRequest(HttpContext context)
        {
            string method = context.Request.Params["method"];
            string jsonResult = string.Empty;
            switch (method)
            {
                // 获取员工账户
                case "GetEmployeeAccount":
                    jsonResult = GetEmployeeAccount(context);
                    break;
                // 取现
                case "Withdraw":
                    jsonResult = Withdraw(context);
                    break;
                // 获取文件完整路径
                case "GetFilepath":
                    jsonResult = GetFilepath(context);
                    break;
                // 获取任务动态
                case "GetTrends":
                    jsonResult = GetTrends(context);
                    break;
                // 获取交易状态列表
                case "GetAllTransactionStatus":
                    jsonResult = GetAllTransactionStatus();
                    break;
                // 更新交易状态
                case "UpdateTransactionStatusByProjectId":
                    jsonResult = UpdateTransactionStatusByProjectId(context);
                    break;
                default:
                    break;
            }
            context.Response.ContentType = "text/json";
            context.Response.Write(jsonResult);
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #region 获取员工账户
        private string GetEmployeeAccount(HttpContext context)
        {
            StringBuilder sbJsonResult = new StringBuilder();
            string employeeID = context.Request.Params["employeeID"];
            EmployeeAccount empAcct = empAcctBll.GetModelList(" employeeID = '" + employeeID + "'").FirstOrDefault();
            TransactionDetailsBLL transactionDetailsBll = new TransactionDetailsBLL();

            //账户余额
            decimal amount = 0m;
            //decimal surplus = 0m;
            // 已发
            decimal paid = 0m;
            // 奖罚，获取交易记录中奖励和处罚之和
            decimal rewardAndAmercement = transactionDetailsBll.GetRewardAndAmercementAmount(employeeID);
            decimal others = transactionDetailsBll.GetOtherAmount(employeeID);
            if (empAcct != null)
            {
                amount = empAcct.AMOUNT ?? 0m;
                //surplus = empAcct.SURPLUSAMOUNT ?? 0m;
                // 已发，取员工账户表中已发的值
                paid = empAcct.PAIDAMOUNT ?? 0m;
                //rewardAndAmercement = empAcct.REWARDANDAMERCEMENTAMOUNT ?? 0m;
                // 其他
                //others = empAcct.OTHERSAMOUNT ?? 0m;
            }
            StringBuilder sbEmpAcct = new StringBuilder();
            sbEmpAcct.Append("[");
            sbEmpAcct.Append("{\"value\":" + amount + ",\"name\":\"剩余\"},");
            sbEmpAcct.Append("{\"value\":" + paid + ",\"name\":\"已发\"},");
            sbEmpAcct.Append("{\"value\":" + Math.Abs(rewardAndAmercement) + ",\"name\":\"奖罚\"},");
            sbEmpAcct.Append("{\"value\":" + Math.Abs(others) + ",\"name\":\"其他\"}");
            sbEmpAcct.Append("]");
            sbJsonResult.Append(sbEmpAcct);
            return sbJsonResult.ToString();
        }
        #endregion

        #region 取现
        private string Withdraw(HttpContext context)
        {
            StringBuilder sbJsonResult = new StringBuilder();
            decimal? withdrawAmount = Convert.ToDecimal(context.Request.Params["withdrawAmount"]);
            string employeeID = context.Request.Params["employeeID"];
            WithdrawDetails withdraw = new WithdrawDetails();
            withdraw.ID = Guid.NewGuid().ToString();
            withdraw.EMPLOYEEID = employeeID;
            withdraw.WITHDRAWAMOUNT = withdrawAmount;
            withdraw.CREATEDATE = DateTime.Now;
            withdraw.ISCONFIRMED = false;
            if (new WithdrawDetailsBLL().Add(withdraw))
            {
                sbJsonResult.Append("{\"result\":\"1\"}");
            }
            else
            {
                sbJsonResult.Append("{\"result\":\"0\"}");
            }
            return sbJsonResult.ToString();
        }
        #endregion

        #region 获取文件路径
        private string GetFilepath(HttpContext context)
        {
            StringBuilder sbJsonResult = new StringBuilder();
            string fileHistoryId = context.Request.Params["fileHistoryId"];
            FileHistory fileHistory = fileHistoryBll.GetModel(fileHistoryId);
            string fileCategoryId = fileHistory.PARENTID;
            FileCategory category = fcBll.GetModel(fileCategoryId);
            string folderName = category.FOLDERNAME;

            string taskNo = string.Empty;
            string returnFileName = string.Empty;
            // 根据任务ID获取任务编号和员工编号，如果有记录说明任务已分配，则需要到员工目录下寻找该文件
            DataTable dtTaskNoAndEmpNo = fileHistoryBll.GetTaskNoAndEmpNoByPrjId(category.PROJECTID).Tables[0];
            // 任务已分配
            if (dtTaskNoAndEmpNo != null && dtTaskNoAndEmpNo.Rows.Count > 0)
            {
                string rootPath = ConfigurationManager.AppSettings["employeePath"].ToString();

                string empNo = dtTaskNoAndEmpNo.AsEnumerable().Select(item => Convert.ToString(item["employeeNo"])).FirstOrDefault();
                string empNoFullPath = Path.Combine(rootPath, empNo);

                taskNo = dtTaskNoAndEmpNo.AsEnumerable().Select(item => Convert.ToString(item["taskNo"])).FirstOrDefault();
                string taskNoFinalFolder = Directory.GetFiles(empNoFullPath, taskNo, SearchOption.AllDirectories).FirstOrDefault();
                if (!string.IsNullOrEmpty(taskNoFinalFolder))
                {
                    returnFileName = Path.Combine(rootPath, empNo, taskNo, folderName);
                }
            }
            // 任务未分配
            else
            {
                Project prj = prjBll.GetModel(category.PROJECTID);
                taskNo = prj.TASKNO;
                string rootPath = ConfigurationManager.AppSettings["taskAllotmentPath"];
                string taskNoFinalFolder = Directory.GetFiles(rootPath, taskNo, SearchOption.TopDirectoryOnly).FirstOrDefault();
                if (!string.IsNullOrEmpty(taskNoFinalFolder))
                {
                    returnFileName = Path.Combine(rootPath, taskNo, folderName);
                }
            }
            using (var output = new StringWriter())
            {
                JSON.Serialize(
                    new
                    {
                        code = 0,
                        msg = "success",
                        filename = returnFileName
                    },
                    output
                );
                sbJsonResult.Append(output.ToString());
            }
            return sbJsonResult.ToString();
        }
        #endregion

        #region 根据员工ID获取任务动态
        private string GetTrends(HttpContext context)
        {
            StringBuilder sbJsonResult = new StringBuilder();
            string employeeID = context.Request.Params["employeeID"];
            List<TaskTrend> lstTrend = new TaskTrendBLL().GetTop10ModelList(" employeeID = '" + employeeID + "'");
            List<TaskTrendInterface> lstResult = new List<TaskTrendInterface>();
            foreach (var item in lstTrend)
            {
                TaskTrendInterface taskTrendInterface = new TaskTrendInterface();
                taskTrendInterface.CreateDate = item.CREATEDATE ?? DateTime.Now;
                taskTrendInterface.FriendlyDate = DateTimeHelper.ChangeTime(item.CREATEDATE ?? DateTime.MinValue);
                taskTrendInterface.TrendContent = item.DESCRIPTION;
                lstResult.Add(taskTrendInterface);
            }
            sbJsonResult.Append(JsonConvert.SerializeObject(lstResult));
            return sbJsonResult.ToString();
        }
        #endregion

        #region 获取所有的交易状态值
        /// <summary>
        /// 获取所有的交易状态值
        /// </summary>
        /// <returns></returns>
        private string GetAllTransactionStatus()
        {
            StringBuilder sbJsonResult = new StringBuilder();
            ConfigureBLL cBll = new ConfigureBLL();
            DataTable dtTransactionStatus = cBll.GetConfig(ConfigTypeName.交易状态.ToString());
            if (dtTransactionStatus != null && dtTransactionStatus.Rows.Count > 0)
            {
                sbJsonResult.Append("[");
                for (int i = 0; i < dtTransactionStatus.Rows.Count; i++)
                {
                    sbJsonResult.Append("{\"key\":\"" + dtTransactionStatus.Rows[i]["configKey"].ToString() + "\",\"value\":\"" + dtTransactionStatus.Rows[i]["configValue"].ToString() + "\"}");
                    if (i != dtTransactionStatus.Rows.Count - 1)
                    {
                        sbJsonResult.Append(",");
                    }
                }
                sbJsonResult.Append("]");
            }
            return sbJsonResult.ToString();
        }
        #endregion

        #region 更新交易状态
        /// <summary>
        /// 更新交易状态
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateTransactionStatusByProjectId(HttpContext context)
        {
            StringBuilder sbJsonResult = new StringBuilder();
            ProjectBLL prjBll = new ProjectBLL();
            string projectId = context.Request.Params["projectId"];
            string newStatus = context.Request.Params["newStatus"];
            Project project = prjBll.GetModel(projectId);
            project.TRANSACTIONSTATUS = newStatus;
            if (prjBll.Update(project))
            {
                sbJsonResult.Append("{\"success\":\"true\"}");
            }
            else
            {
                sbJsonResult.Append("{\"success\":\"false\"}");
            }
            return sbJsonResult.ToString();
        }
        #endregion
    }
}