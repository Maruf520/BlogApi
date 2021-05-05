# BlogApi
## Add migration
Add-Migration -StartupProject Blog.Api -Context BlogContext "inital"

## Update-Database
update-database -StartupProject Blog.Api -Context BlogContext "initial"
