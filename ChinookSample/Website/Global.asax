﻿<%@ Application Language="C#" %>
<%@ Import Namespace="Website" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="ChinookSystem.Security" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e)
    {
        RouteConfig.RegisterRoutes(RouteTable.Routes);
        BundleConfig.RegisterBundles(BundleTable.Bundles);

        //Load the startup roles for chinook
        var roleManager = new RoleManager();
        roleManager.AddStartUpRoles();
        
        //Load the startup roles for chinook
        var userManager = new UserManager();
        userManager.AddWebmaster();
    }

</script>
