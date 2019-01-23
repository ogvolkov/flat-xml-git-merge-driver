XML files are a very common source of merge conflicts in git, even though in many cases, such as .resx and .csproj files, logically the resolution would be as easy as just taking both changes.

I'm exploring whether it is possible to write a custom git merge driver which would automatically merge simple XML files.
