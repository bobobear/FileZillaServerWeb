//global var
//var urlserver = "http://localhost:59649";
var urlserver = "";

var funcList = {};

//init releated
funcList["init"] = {
    func: init,
    interface: ""
};

//file
funcList["getFileTabs"] = {
    func: getFileTabsByProjectid,
    interface: urlserver + "/HttpHandler/FileHandler.ashx?FuncName=GetCategories"
};

// 获取尚未创建完成稿的修改任务
funcList["getExistFileNotFinishCategory"] = {
    func: getExistFileNotFinishCategory,
    interface: urlserver + "/HttpHandler/FileHandler.ashx?FuncName=GetExistFileNotFinishCategory"
};

// 获取文件
funcList["getFiles"] = {
    func: getFiles,
    interface: urlserver + "/HttpHandler/FileHandler.ashx?FuncName=GetFilesByProject"
};

// Delete file
funcList["deleteFile"] = {
    func: deleteFile,
    interface: urlserver + "/HttpHandler/FileHandler.ashx?FuncName=DeleteFile"
};

// Download file
funcList["downloadFile"] = {
    func: downloadFile,
    interface: urlserver + "/HttpHandler/FileHandler.ashx?FuncName=DownloadFile"
};

// Preview file
funcList["previewFile"] = {
    func: previewFile,
    interface: urlserver + "/HttpHandler/FileHandler.ashx?FuncName=PreviewFile"
};

// Share link
funcList["ShareLink"] = {
    func: ShareLink
};

// 判断是否进行过提醒
funcList["GetIsRemind"] = {
    func: GetIsRemind,
    interface: urlserver + "/HttpHandler/FileHandler.ashx?FuncName=GetIsRemind"
};

// 发送钉钉提醒
funcList["sendDingtalkMessage"] = {
    func: sendDingtalkMessage,
    interface: urlserver + "/HttpHandler/FileHandler.ashx?FuncName=SendDingtalkMessage"
};

//when project id changed
funcList["refreshProject"] = {
    func: refreshProject,
    interface: ""
};


/**
 * history related
 * 
 */

funcList["getHistoryData"] = {
    func: getHistoryData,
    interface: urlserver + "/HttpHandler/FileHandler.ashx?FuncName=GetProjectOperationLogs"
};

/**
 * add new tab related
 * 
 */
funcList["getCategoryList"] = {
    func: getCategoryList,
    interface: urlserver + "/HttpHandler/FileHandler.ashx?FuncName=addCategory"
};

funcList["getReplytodata"] = {
    func: getReplytodata,
    interface: urlserver + "/HttpHandler/FileHandler.ashx?FuncName=AddFileCategory"
};

funcList["addNewTab"] = {
    func: addNewTab,
    interface: urlserver + "/HttpHandler/FileHandler.ashx?FuncName=AddFileCategory"
};

funcList["addReplytoTab"] = {
    func: addReplytoTab,
    interface: urlserver + "/HttpHandler/FileHandler.ashx?FuncName=GetReplyToTab"
};
