﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="aa_WebForm2.aspx.cs" Inherits="FileZillaServerWeb.aa_WebForm2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
            <link href="Scripts/bootstrap4/css/bootstrap.css" rel="stylesheet" />
    <link href="Scripts/webuploader/webuploader.css" rel="stylesheet" />
</head>
<body>
    <form id="form1">


            <div id="project" class="container" style=" clear:both;">

            <div id="meun">
                <div class="row">
                    <div class="col -12" style="text-align: left;">
                        <div class="btn-group btn-group-lg">
                            <button type="button" class="btn btn-default btn-primary" @click="changeTab(false,true,false)">File</button>
                            <button type="button" class="btn btn-default btn-success" @click="changeTab(true,false,false)">History</button>
                            <button type="button" class="btn btn-default" @click="changeTab(false,false,true)">Add A New Tab</button>
                        </div>
                    </div>
                </div>
            </div>


            <div id="projectfile" v-show="showfile">
                File Tab
                <!-- Tab List -->
                <div class="row">
                    <div class="col -12">
                        <div class="btn-group btn-group-sm">
                            <!-- change file list -->
                            <button :key="item.Id" :title="item.description" :filetype="item.Id" class="btn btn-default btn-primary " v-for="item in projectfile.filetabs">
                                {{item.title}}
                            </button>
                        </div>
                    </div>
                </div>

                <!-- File List -->
                <div class="row">
                    <div class="col -12">
                        <div id="showfiles" style="text-align: left">
                            <table class="table table-bordered table-hover  table-striped">
                                <tbody>
                                    <tr v-for=" file in projectfile.files">
                                        <td>
                                            图标:
                                            <!-- <span :title="file.filedesc">{{file.fileName}}</span>
                                            <span :title="">{{file.filePath}}</span>
                                            <a href="dotPeek.rar">rar</a>
                                            <a href="uploadfile.html">html</a> -->
                                        </td>
                                        <td>
                                            上传时间
                                        </td>
                                        <td>
                                            <div class="btn-group btn-group-sm">
                                                <button type="button" class="btn btn-default btn-primary">delete</button>
                                                <button type="button" class="btn btn-default btn-success">download</button>
                                            </div>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>


                <!-- Upload File -->


            </div>


            <div id="projecthistory" v-show="showhistory">
                history
                <div class="row">
                    <div class="col-12">
                        <h4>Project ID:{{projectid}}</h4>
                        <div>
                            <table class="table table-bordered table-hover  table-striped">
                                <!-- 表头 -->
                                <thead>
                                <td>
                                    OperateDate
                                </td>
                                <td>
                                    OperateUser
                                </td>
                                <td>
                                    operateContent
                                </td>
                                </thead>

                                <!-- 内容 -->
                                <tbody>
                                    <tr v-for=" item in projecthistory.data " :key="item.id ">
                                        <td>
                                            {{item.operateDate|convTime}}
                                        </td>
                                        <td>
                                            {{item.operateUser}}
                                        </td>
                                        <td>
                                            {{item.operateContent}}
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>

                    </div>
                </div>
            </div>


            <div id="addtab" v-show="showaddtab">
                add new tab
                <div class="row">
                    <div class="col-12">
                        <form class="form-inline" role="form">
                            <div class="form-group">
                                <label for="category">选择列表 : </label>
                                <select class="form-control" name="category" id="category" v-model="newtab.categoryselected" @change="categoryChange()">
                                    <option v-for="item in newtab.category" :value="item.key">{{item.value}}</option>
                                </select>
                                <span> {{newtab.categoryselected}}</span>
                            </div>

                            <div class="form-group" v-show="showreply">
                                <label for="replyto">回复 : </label>
                                <select class="form-control" name="replyto" id="replyto" v-model="newtab.replytoselected">
                                    <option v-for="item in newtab.replyto" :value="item.Id">
                                        {{item.Title}}
                                    </option>
                                </select>
                                <span> {{newtab.replytoselected}}</span>
                            </div>

                            <div class="form-group">
                                <label for="category">描述 : </label>
                                <input type="text" name="tabdesc" id="tabdesc" v-model="newtab.desc">
                            </div>
                            <button type="button" class="btn btn-default" @click="addTab()" id="add">新增</button>
                        </form>
                        <div class="form-group">
                            <p>{{this.newtab.returnmessage}}</p>
                        </div>

                    </div>
                </div>
            </div>
        </div>


    </form>



    
            <script src="<%= ResolveUrl("~/Scripts/jquery-3.3.1.min.js") %>"></script>
    <script src="Scripts/bootstrap4/js/bootstrap.js"></script>
    <script src="Scripts/webuploader/webuploader.js"></script>

    <script src="Scripts/vue/vue.js"></script>
    <script src="Scripts/ylyj/employeehome/func.js"></script>
    <script src="Scripts/ylyj/employeehome/settings.js"></script>
    <script src="Scripts/ylyj/employeehome/vuepage.js"></script>
</body>
</html>
