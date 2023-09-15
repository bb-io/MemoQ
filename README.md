# Blackbird.io MemoQ

Blackbird is the new automation backbone for the language technology industry. Blackbird provides enterprise-scale automation and orchestration with a simple no-code/low-code platform. Blackbird enables ambitious organizations to identify, vet and automate as many processes as possible. Not just localization workflows, but any business and IT process. This repository represents an application that is deployable on Blackbird and usable inside the workflow editor.

## Introduction

<!-- begin docs -->

MemoQ offers flexible translation and localization management solutions tailored to enterprises, language service providers, and translators.. This MemoQ application primarily centers around project and file management.

## Connecting

1.  Navigate to apps and search for MemoQ . If you cannot find MemoQ then click _Add App_ in the top right corner, select MemoQ and add the app to your Blackbird environment.
2.  Click _Add Connection_.
3.  Name your connection for future reference e.g. 'My client'.
4.  Copy API key you got from MemoQ and instance URL and paste it to the appropriate fields in the BlackBird
5.  Click _Connect_.
6.  Confirm that the connection has appeared and the status is _Connected_.

## Actions

### Analyses

-   **Get document/project analysis**

### Files

-   **List project documents** returns a list of all documents related to a specified project.
-   **Slice document** slices specific document based on the specified options.
-    **Assign document to user** assigns the document to a specific user.
-   **Get/delete/overwrite/deliver document**
-   **Import/Export document**
-   **Export document as XLIFF** exports and downloads the translation document as XLIFF (MQXLIFF) bilingual.
-   **Apply translated content to updated source**

### Groups

- **List groups** returns a list of all groups

### Packages

-  **Create delivery package** creates a new delivery package from document IDs.
-  **Deliver package** delivers a specific package.

### Projects

- **List projects** returns a list of all projects.
- **Get/create/delete/distribute project **
- **Create project from package/template** creates a new project based on a specified template/package.
- **Add target language to project** adds target language to a specific project.

### Translation memories

-  **List translation memories** returns a list of all translation memory.
- **Get/create/update/delete**
- **Import TMX file** imports TMX file to the translation memory.
- **Import translation memory scheme from XML** imports translation memory metadata scheme from an XML file.

### Users

-  **List users** returns a list of all users.
-  **Get/create/delete user**

## Events

-   **On document delivered** is triggered when any project document was delivered.

## Missing features

In the future we can add actions for:

-   Termbases
-   Tasks
-   Resources

## Feedback

Feedback to our implementation of MemoQ is always very welcome. Reach out to us using the established channels or create an issue.

<!-- end docs -->
