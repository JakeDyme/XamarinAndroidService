##Setting up binding to service:
### Fixing Assembly error:
Add reference to Java.Interop
https://stackoverflow.com/a/37792623/5237938
- Add Reference to a project that can list the references.
- Look in Assemblies for the Lib in that project and get the filepath from the properties.
- In your .NET Standard project, "Add Project Reference", "Browse", enter the directory and select the Lib

