﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Admin_Security_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="row jumbotron">
        <h1>User and Role Administration</h1>
    </div>

    <div class="row">
        <div class="col-md-12">
            <!-- Nav Tabs -->
            <ul class="nav nav-tabs">
                <li class="active"><a href="#users" data-toggle="tab">Users</a></li>
                <li><a href="#roles" data-toggle="tab">Roles</a></li>
                <li><a href="#unregistered" data-toggle="tab">UnRegisterd Users</a></li>
            </ul>

            <!-- Tab Content Area-->
            <div class="tab-content">
                <!-- User tab -->
                <div class="tab-pane fade in active" id="users">
                    
                </div> <!-- End of: User tab-->

                <!-- Roles tab -->
                <div class="tab-pane fade" id="roles">
                    <%--<asp:ListView ID="RoleListView" runat="server"
                        DataSourceID="RoleListViewODS"
                        ItemType="RoleProfile"
                        InsetItemPosition="LastItem">
                            <EmptyDataTemplate>
                                <span>No Security Roles have been set up</span>
                            </EmptyDataTemplate>

                            <LayoutTemplate>
                                <div class="col-sm-3 h4">Action</div>
                                <div class="col-sm-3 h4">Role</div>
                                <div class="col-sm-6 h4">Users</div>
                            </LayoutTemplate>

                            <ItemTemplate>
                                <div class="col-sm-3">
                                    <asp:LinkButton ID="RemoveRole" runat="server">Remove</asp:LinkButton>
                                </div>

                                <div class="col-sm-3">
                                    <%Item.RoleName%>
                                </div>

                                <div class="col-sm-6">
                                    <asp:Repeater ID="RoleUsers" runat="server"
                                                  DataSource="<%Item.UserNames%>"
                                                  ItemType="System.String">
                                        <ItemTemplate>
                                            <%Item%>
                                        </ItemTemplate>
                                        <SeparatorTemplate>, </SeparatorTemplate>
                                    </asp:Repeater>
                                </div>
                            </ItemTemplate>

                            <InsertItemTemplate>
                                <div class="col-sm-3">
                                    <asp:LinkButton ID="InsertRole" runat="server">Insert</asp:LinkButton>&nbsp;&nbsp;
                                    <asp:LinkButton ID="Cancel" runat="server">Cancel</asp:LinkButton>
                                </div>

                                <div class="col-sm-3">
                                    <asp:TextBox ID="RoleName" runat="server"
                                                 text='<%BindItem.RoleName%>'
                                                 placeholder="Role Name">

                                    </asp:TextBox>
                                </div>
                            </InsertItemTemplate>
                    </asp:ListView>
                    <asp:ObjectDataSource ID="RoleVistViewODS" runat="server"></asp:ObjectDataSource>--%>
                </div> <!-- End of: Roles tab-->

                <!-- UnRegistered Users tab -->
                <div class="tab-pane fade" id="unregistered">
                    <h2>UnRegistered</h2>
                </div> <!-- End of: UnRegistered Users tab-->
            </div>
        </div>
    </div>
</asp:Content>

