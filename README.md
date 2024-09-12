# Web Servers Monitoring System

### This Application was accomplished using two projects: <br />
#### &nbsp; &nbsp; &nbsp;  1. WebServersDiagnose-WebApi <br />
#### &nbsp; &nbsp; &nbsp;  2. WebServersDiagnose-Diagnose <br /> <br />

### 1. Project - WebServersDiagnose-WebApi

&nbsp; &nbsp; It's the endpoints REST API. <br />
&nbsp; &nbsp; This API makes CRUD operations on "WebServers database" / "tblWebServers table". <br />
&nbsp; &nbsp; "tblWebServers table" holds the Web Services list and their info. <br /><br />
&nbsp; &nbsp; Languages:&nbsp; ASP.NET Core WebApi <br /><br />

### 2. Project - WebServersDiagnose-Diagnose

&nbsp; &nbsp; It's the automated worker that will monitor the webservers status. <br />
&nbsp; &nbsp; The application Design Patterns are "Observer" and "Iterator". <br />
  #### 2.1 Observer Pattern: 
  &nbsp; &nbsp;  "Observee" -> WebServersDiagnose project / DataTableObservee.cs file <br />
  &nbsp; &nbsp;  DataTableObservee.cs notify WebServersDispatcher.cs for 
    "WebServers database" / "tblWebServers table"  <br /> 
 &nbsp; &nbsp;  content changes.  <br />
  &nbsp; &nbsp;  "Observer" -> WebServersDiagnose project / WebServersDispatcher.cs file <br />
  #### 2.2 Iterator:
  &nbsp; &nbsp; &nbsp;  "Iterator" -> WebServersDiagnose project / WebServersIterator.cs file <br />
  &nbsp; &nbsp; &nbsp;  "Aggregate" -> DP project / IAggregate.cs + WebServersDispatcher.cs files <br /><br />
&nbsp; &nbsp; &nbsp; Languages: .NET C# / WinForms <br />
&nbsp; &nbsp; &nbsp; Databases: MS Sql Server <br /><br />

### 3. Run WebServersDiagnose-WebApi Postman

&nbsp; &nbsp; 3.1 Http calls JSON file -> WebServersApi.postman_collection.json file <br />
&nbsp; &nbsp; 3.2 Http calls organizer -> PostmanUrlsMapping.xlsx file <br />
&nbsp; &nbsp; 3.3 Database backup -> WebServers.bak file <br />
&nbsp; &nbsp; 3.4 Database scripts -> WebServers.sql.sql file <br />

