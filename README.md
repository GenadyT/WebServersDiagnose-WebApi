## Web Servers Monitoring System

&nbsp; &nbsp; This Application was accomplished using two projects: <br />
&nbsp; &nbsp; &nbsp; &nbsp;  1. WebServersDiagnose-Diagnose <br />
&nbsp; &nbsp; &nbsp; &nbsp;  2. WebServersDiagnose-WebApi <br />

### 1. Project - WebServersDiagnose-Diagnose

&nbsp; &nbsp; It's the automated worker that will monitor the webservers status. <br />
&nbsp; &nbsp; &nbsp; &nbsp; Languages: .NET C# / WinForms <br />
&nbsp; &nbsp; &nbsp; &nbsp; Databases: MS Sql Server <br />

&nbsp; &nbsp; The application Design Patterns are Observer and Iterator. <br />
&nbsp; &nbsp; &nbsp; &nbsp;  Observer: <br />
  &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;  Observee -> WebServersDiagnose project / DataTableObservee.cs file <br />
  &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;  Observer -> WebServersDiagnose project / WebServersDispatcher.cs file <br />
&nbsp; &nbsp; &nbsp; &nbsp;  Iterator:  <br />
  &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;  Iterator -> WebServersDiagnose project / WebServersIterator.cs file <br />
  &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;  Aggregate -> DP project / IAggregate.cs + WebServersDispatcher.cs files <br />

### 2. Project - WebServersDiagnose-WebApi

&nbsp; &nbsp; It's the endpoints REST API. <br />
&nbsp; &nbsp; Languages: ASP.NET Core WebApi <br />

### 3. Run WebServersDiagnose-WebApi Postman

&nbsp; &nbsp; 3.1 Http calls JSON file -> WebServersApi.postman_collection.json file <br />
&nbsp; &nbsp; 3.2 Http calls organizer -> PostmanUrlsMapping.xlsx file <br />
&nbsp; &nbsp; 3.3 Database backup -> WebServers.bak file <br />
&nbsp; &nbsp; 3.4 Database scripts -> WebServers.sql.sql file <br />
