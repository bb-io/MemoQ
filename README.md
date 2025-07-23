# Blackbird.io memoQ

Blackbird is the new automation backbone for the language technology industry. Blackbird provides enterprise-scale automation and orchestration with a simple no-code/low-code platform. Blackbird enables ambitious organizations to identify, vet and automate as many processes as possible. Not just localization workflows, but any business and IT process. This repository represents an application that is deployable on Blackbird and usable inside the workflow editor.

## Introduction

<!-- begin docs -->

memoQ offers flexible translation and localization management solutions tailored to enterprises, language service providers, and translators. This memoQ application primarily centers around project and file management.

## Before setting up

Before you can connect you need to make sure that:

- You have access to a memoQ instance.
- Your memoQ instance has the WS API enabled and you have an API key.
- If your memoQ instance requires whitelisting then ask us about our Blackbird _sandbox_ IP addresses.

You can read more about how to set up your WS API [here](https://docs.memoq.com/current/en/memoQ-server-deployment-tool/deptool-install-memoq-server.html) under the section 'Secure the Web Service API'.

## Connecting

1.  Navigate to apps and search for memoQ . If you cannot find memoQ then click _Add App_ in the top right corner, select memoQ and add the app to your Blackbird environment.
2.  Click _Add Connection_.
3.  Name your connection for future reference e.g. 'My memoQ'.
4.  Add the URL pointing to your memoQ instance API. Usually this is your instance URL, port with the added `/memoqservices` but this can be different (see image below).
5.  Add your API key. If your memoQ service doesn't require an API key, you can input 'NONE'. We will process it under the hood and exclude it from requests.
6.  Click _Connect_.
7.  Confirm that the connection has appeared and the status is _Connected_.

![1695644590394](image/README/1695644590394.png)

## Actions

### Analyses

- **Get file/project analysis**.

### Files

- **Search project files** returns the files currently in a project.
- **Slice file** slices a file based on the specified options.
- **Assign file to user** assigns the file to a specific user.
- **Get/delete/overwrite/deliver file**.
- **Upload/Download file** uploads/downloads file to the project. Make sure your file name contains extension, otherwise the action will fail. Also has optional inputs for external file ID, filter config, preview creation and importing embedded images and objects.
- **Apply translated content to updated source**.
- **File exists** checks if a file with provided properties exists in the project.

### XLIFFs

- **Download XLIFF file**: Exports and downloads an XLIFF file. If you set 'Use MQXLIFF' optional input to true, it will return an mqxliff file; if it's set to false, it will return an XLIFF 1.2 version file. By default, it is set to false.

- **Upload XLIFF file**: Uploads and imports an XLIFF file to a project. XLIFF is the standard for exchanging localization data. Currently, it supports XLIFF versions 1.2 and 2.1, and you can also import mqxliff files.
	- 'Re-import file ID': The unique identifier of the original file you want to overwrite. This is required if you want to reimport a file. If specified, the action will try to reimport the file instead of a simple import.

	- 'Update segment statuses': An optional input that indicates whether to update segment statuses during the import operation. If set to false, it will only reimport the existing file; if set to true, it will match the IDs of segments and update the segment status to 'Edited' if the target value of the segment is different, and also update the target text of the segment.

	- 'Path to set as import path': An optional input that represents the path to set as the import path. If you want to reimport the file it's required, you can find this path from **Download XLIFF file** action by exporting specific file, and you can use 'Export path' property here (it looks like: '\\en-uk_ukr.xliff')

- **Update file from XLIFF file**: Update a project file from an XLIFF file. As optional input, you can choose whether locked or confirmed segments should be updated. By default, all segments with changes will be updated.

### Packages

- **Create delivery package** creates a new delivery package from file IDs.
- **Deliver package** delivers a specific package.

### Projects

- **Search projects** Search through your memoQ projects.
- **Get/create/delete/distribute project**.
- **Create project from package/template** creates a new project based on a specified template/package.
- **Update project** updates details of a specified project.
- **Add target language to project** adds target language to a specific project.
- **Get resources assigned to project** returns a list of all resources assigned to a project. You specify the type of resource you are looking for e.g. MT engine or TM.
- **Add resource to project** add a new resource to a project.
- **Start pretranslation task** starts a pretranslation task for a specific project. 
- **Add glossary to project** add termbase to a specific project by ID.
- **Pretranslate files** This action allows you to pretranslate files in a specific project. Pretranslation is a process where the system automatically fills in the translations for segments in a file based on certain criteria. This can significantly speed up the translation process. Parameters:  
	- 'File IDs': This parameter is used to specify the unique identifiers of the files you want to pretranslate. If you don't specify any file ID, the action will pretranslate all files in the project.
	- 'Target languages': This parameter is used to specify the target languages for pretranslation. If you don't specify any target languages, the action will pretranslate all target languages in the project.
	- 'Lock': This optional parameter, when set to true, locks the pretranslated segments to prevent further editing. By default, this is set to true.
	- 'Confirm lock pretranslated': This optional parameter determines the state of segments that should be confirmed and locked during pretranslation. By default, this is set to 'ExactMatch'.
	- 'Pretranslate lookup behavior': This optional parameter determines the behavior of the pretranslation lookup process.  
	- 'Use MT': This optional parameter, when set to true, enables the use of Machine Translation (MT) during pretranslation.  
	- 'Translation memories IDs': This optional parameter is used to specify the unique identifiers of the translation memories to be used during pretranslation.  
	- 'Include numbers': This optional parameter, when set to true, includes numbers in the pretranslation. By default, this is set to true.  
	- 'Change case': This optional parameter, when set to true, changes the case of the pretranslated text. By default, this is set to false.  
	- 'Include auto translations': This optional parameter, when set to true, includes auto translations in the pretranslation. By default, this is set to true.  
	- 'Include fragments': This optional parameter, when set to true, includes fragments in the pretranslation. By default, this is set to true.  
	- 'Include non-translatables': This optional parameter, when set to true, includes non-translatable text in the pretranslation. By default, this is set to true.  
	- 'Include term bases': This optional parameter, when set to true, includes term bases in the pretranslation. By default, this is set to true.  
	- 'Minimum coverage': This optional parameter is used to specify the minimum coverage for pretranslation. By default, this is set to 50.  
	- 'Coverage type': This optional parameter is used to specify the type of coverage for pretranslation. By default, this is set to 'Not full'.  
	- 'Only unambiguous matches': This optional parameter, when set to true, only includes unambiguous matches in the pretranslation. By default, this is set to true.
	- 'Final translation state': This optional parameter is used to specify the final translation state for pretranslated segments. By default, this is set to 'No change'.

### Project custom fields
- **Get project custom fields** Gets all custom metadata fields for a specific project
- **Get custom field value** Gets value of a specific custom metadata field
- **Set custom field value** Sets the value of a specific custom metadata field
- **Add new custom field** Adds a custom metadata field to the specified project

### Translation memories

- **Search translation memories** returns all translation memories given certain filters.
- **Get/create/update/delete**.
- **Import TMX file** imports TMX file to the translation memory.
- **Import translation memory scheme from XML** imports translation memory metadata scheme from an XML file.

### Term bases

- **Get term bases assigned to project** Gets a list of term bases assigned to a project for a specific target language.
- **Import or update glossary** This action allows importing a '.tbx' file to update or create the termbase. In case of creating termbase the following fields are required: `Glossary file`, `Glossary name`. In case of updating the termbase the following fields are required: `Glossary file`, `Existing termbase ID`.
- **Export glossary** exports an existing term base. This action accepts an optional input parameter called _Include forbidden terms_ which defaults to _False_. When set to _False_, terms with _Forbidden_ status are excluded from the resulting export.
- **Update existing glossary** updates an existing termbase with a new. There are optional inputs `AllowAddNewLanguages`(where you can give permission to add new languages by deafault is `true`) and `OverwriteEntiesWithSameId`(where you can overwrite enties with same ID by default is `false`) 

**Import glossary** and **Export glossary** support both regular term bases and QTerm. It's essential to note that the current implementation only facilitates basic imports/exports, covering fundamental information like terms, languages, and definitions. However, additional details such as domain, usage examples, client, project, and other details are not included in the glossaries.

Another important consideration is that our glossaries implementation adheres to the ISO 639-1 standard language codes, in contrast to memoQ. If there is no corresponding ISO 639-1 language code for a language supported by memoQ, our glossaries will utilize memoQ's ISO 639-3 language code. This can result in incompatibility with other systems if such languages are present in a glossary. However, it's worth mentioning that you will still be able to manipulate these glossaries within memoQ.

### Users

- **Get/create/delete user**.

## Events

- **On file delivered** is triggered when any project file was delivered.
- **On projects created** is triggered when new projects are created.
- **On project status changed** is triggered when status of a specific project has changed.
- **On task status changed** is triggered when status of a specific task has changed.

## Missing features

In the future we can add actions for:

- Tasks

## Feedback

Feedback to our implementation of memoQ is always very welcome. Reach out to us using the [established channels](https://www.blackbird.io/), or create an issue.

<!-- end docs -->
