# PM-REST-From-CSharp
A simple example of C# code connecting to ProcessMaker via the REST API

Just import the ConsoleApplication1 project into VS. Make sure to modigy 
Program.cs so that it matches your PM server settings. Values you need 
to modify:

Under the // Define PM server parameters comment:
- workspace
- protocol
- hostname

Under the // Perform initial request to get an OAuth2 token comment:
- client_id
- client_secret
- username
- password



