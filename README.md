# Blackbird.io memoQ

Blackbird is the new automation backbone for the language technology industry. Blackbird provides enterprise-scale automation and orchestration with a simple no-code/low-code platform. Blackbird enables ambitious organizations to identify, vet and automate as many processes as possible. Not just localization workflows, but any business and IT process. This repository represents an application that is deployable on Blackbird and usable inside the workflow editor.

## Introduction

<!-- begin docs -->

memoQ offers flexible translation and localization management solutions tailored to enterprises, language service providers, and translators. This memoQ application primarily centers around project and file management.

## Before setting up

Before you can connect you need to make sure that:

- You have access to a memoQ instance.
- Your memoQ instance has the API enabled and you have an API key.
- If your memoQ instance requires whitelisting then ask us about our Blackbird _sandbox_ IP addresses.

## Connecting

1.  Navigate to apps and search for memoQ . If you cannot find memoQ then click _Add App_ in the top right corner, select memoQ and add the app to your Blackbird environment.
2.  Click _Add Connection_.
3.  Name your connection for future reference e.g. 'My memoQ'.
4.  Add the URL pointing to your memoQ instance API. Usually this is your instance URL, port with the added `/memoqservices` but this can be different (see image below).
5.  Add your API key.
6.  Click _Connect_.
7.  Confirm that the connection has appeared and the status is _Connected_.

![1695644590394](image/README/1695644590394.png)

## Actions

### Analyses

- **Get document/project analysis**.

### Files

- **List project documents** returns a list of all documents related to a specified project.
- **Slice document** slices specific document based on the specified options.
- **Assign document to user** assigns the document to a specific user.
- **Get/delete/overwrite/deliver document**.
- **Import/Export document** uploads/downloads file to the project. Make sure your file name contains extension, otherwise the action will fail.
- **Export document as XLIFF** exports and downloads the translation document as XLIFF (MQXLIFF) bilingual.
- **Apply translated content to updated source**.

### Groups

- **List groups** returns a list of all groups.

### Packages

- **Create delivery package** creates a new delivery package from document IDs.
- **Deliver package** delivers a specific package.

### Projects

- **List projects** returns a list of all projects.
- **Get/create/delete/distribute project**.
- **Create project from package/template** creates a new project based on a specified template/package.
- **Add target language to project** adds target language to a specific project.

### Translation memories

- **List translation memories** returns a list of all translation memory.
- **Get/create/update/delete**.
- **Import TMX file** imports TMX file to the translation memory.
- **Import translation memory scheme from XML** imports translation memory metadata scheme from an XML file.

### Term bases

- **Import glossary** imports a term base.
- **Export glossary** exports an existing term base. This action accepts an optional input parameter called _Include forbidden terms_ which defaults to _False_. When set to _False_, terms with _Forbidden_ status are excluded from the resulting export.

**Import glossary** and **Export glossary** support both regular term bases and QTerm. It's essential to note that the current implementation only facilitates basic imports/exports, covering fundamental information like terms, languages, and definitions. However, additional details such as domain, usage examples, client, project, and other details are not included in the glossaries.

Another important consideration is that our glossaries implementation adheres to the ISO 639-1 standard language codes, in contrast to memoQ. If there is no corresponding ISO 639-1 language code for a language supported by memoQ, our glossaries will utilize memoQ's ISO 639-3 language code. This can result in incompatibility with other systems if such languages are present in a glossary. However, it's worth mentioning that you will still be able to manipulate these glossaries within memoQ.

### Users

- **List users** returns a list of all users.
- **Get/create/delete user**.

## Events

- **On document delivered** is triggered when any project document was delivered.

## Missing features

In the future we can add actions for:

- Tasks
- Resources

## Feedback

Feedback to our implementation of memoQ is always very welcome. Reach out to us using the [established channels](https://www.blackbird.io/), or create an issue.

<!-- end docs -->
