# Test

Online Missing Vehicle Project
This is a project developed using ASP.NET Core, utilizing the code-first approach for the database. The project allows users to register, login, add complaints about missing vehicles, view their complaint status, and subscribe for updates on their complaint status. Admin users have additional privileges to manage complaints and view subscription statuses.

Getting Started
To get started with the project, follow these steps:

Download Zip: Download the project zip file from the GitHub repository.

Extract Files: Extract the contents of the zip file to your local machine.

Database Migration: Open the solution in Visual Studio and navigate to Tools > NuGet Package Manager > Package Manager Console. In the Package Manager Console, run the following command to update the database:
update-database

Run Project: Build and run the project in Visual Studio.

User Registration: Register as an admin or a regular user. The first registered user will automatically receive admin privileges, while subsequent users will have regular user roles.

Add Complaints: After logging in, navigate to the "Add Complaint" section and click on the "Add Complaint" button to add a complaint about a missing vehicle.

View Complaint Status: Your added complaints will be displayed on the index page of the "Add Complaint" section. You can also view your complaint status on the home page.

Subscribe for Updates: You can subscribe for updates about your complaint status.

Admin Privileges: Log in with an admin account to access the complaints section. Here, you can change the status of complaints and view subscription statuses on the admin dashboard.
