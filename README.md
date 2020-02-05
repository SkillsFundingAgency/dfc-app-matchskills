# DFC.App.MatchSkills

ASP.Net Core 3 applet which plugs in to the Composite-UI architecture for "Match Your Skills to a Career".

For details on the Composite-UI see [https://github.com/SkillsFundingAgency/dfc-composite-shell](https://github.com/SkillsFundingAgency/dfc-composite-shell)

## Build & Deployment Piplelines

### dfc-app-matchskills

| [Branch](https://github.com/SkillsFundingAgency/dfc-app-matchskills) | [Build](https://sfa-gov-uk.visualstudio.com/Digital%20First%20Careers/_build?definitionId=1944&_a=summary) | [Sonar](https://sonarcloud.io/dashboard?branch=dev&id=SonarCloud.SkillsFundingAgency.dfc-app-matchskills) | [Deploy](https://sfa-gov-uk.visualstudio.com/Digital%20First%20Careers/_release?_a=releases&view=mine&definitionId=116)
--- | --- | --- | ---
Dev   | [![Build Status](https://sfa-gov-uk.visualstudio.com/Digital%20First%20Careers/_apis/build/status/Personalisation/dfc-app-matchskills?branchName=dev)](https://sfa-gov-uk.visualstudio.com/Digital%20First%20Careers/_build/latest?definitionId=1944&branchName=dev)       | N/A | N/A
Master| [![Build Status](https://sfa-gov-uk.visualstudio.com/Digital%20First%20Careers/_apis/build/status/Personalisation/dfc-app-matchskills?branchName=master)](https://sfa-gov-uk.visualstudio.com/Digital%20First%20Careers/_build/latest?definitionId=1944&branchName=master) | [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=SonarCloud.SkillsFundingAgency.dfc-app-matchskills&metric=alert_status)](https://sonarcloud.io/dashboard?id=SonarCloud.SkillsFundingAgency.dfc-app-matchskills) | https://dfc-dev-pers-matchskills-as.azurewebsites.net/

### Dependencies

#### dfc-personalisation-common-pkg-netcore

| [Branch](https://github.com/SkillsFundingAgency/dfc-personalisation-common-pkg-netcore) | [Build](https://sfa-gov-uk.visualstudio.com/Digital%20First%20Careers/_build?definitionId=1922&_a=summary) | [Sonar](https://sonarcloud.io/dashboard?branch=dev&id=SonarCloud.SkillsFundingAgency.dfc-personalisation-common-pkg-netcore) | Deploy
--- | --- | --- | ---
Dev   | [![Build Status](https://sfa-gov-uk.visualstudio.com/Digital%20First%20Careers/_apis/build/status/Personalisation/dfc-personalisation-common-pkg-netcore?branchName=dev)](https://sfa-gov-uk.visualstudio.com/Digital%20First%20Careers/_build/latest?definitionId=1922&branchName=dev)       | N/A | N/A
Master| [![Build Status](https://sfa-gov-uk.visualstudio.com/Digital%20First%20Careers/_apis/build/status/Personalisation/dfc-personalisation-common-pkg-netcore?branchName=master)](https://sfa-gov-uk.visualstudio.com/Digital%20First%20Careers/_build/latest?definitionId=1922&branchName=master) | [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=SonarCloud.SkillsFundingAgency.dfc-personalisation-common-pkg-netcore&metric=alert_status)](https://sonarcloud.io/dashboard?id=SonarCloud.SkillsFundingAgency.dfc-personalisation-common-pkg-netcore) | https://www.nuget.org/packages/DFC.Personalisation.Common/

#### dfc-personalisation-domain-pkg-netcore

| [Branch](https://github.com/SkillsFundingAgency/dfc-personalisation-domain-pkg-netcore) | [Build](https://sfa-gov-uk.visualstudio.com/Digital%20First%20Careers/_build?definitionId=1943&_a=summary) | [Sonar](https://sonarcloud.io/dashboard?branch=dev&id=SonarCloud.SkillsFundingAgency.dfc-personalisation-domain-pkg-netcore) | Deploy
--- | --- | --- | ---
Dev   | [![Build Status](https://sfa-gov-uk.visualstudio.com/Digital%20First%20Careers/_apis/build/status/Personalisation/dfc-personalisation-domain-pkg-netcore?branchName=dev)](https://sfa-gov-uk.visualstudio.com/Digital%20First%20Careers/_build/latest?definitionId=1943&branchName=dev)       | N/A | N/A
Master| [![Build Status](https://sfa-gov-uk.visualstudio.com/Digital%20First%20Careers/_apis/build/status/Personalisation/dfc-personalisation-domain-pkg-netcore?branchName=master)](https://sfa-gov-uk.visualstudio.com/Digital%20First%20Careers/_build/latest?definitionId=1943&branchName=master) | [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=SonarCloud.SkillsFundingAgency.dfc-personalisation-domain-pkg-netcore&metric=alert_status)](https://sonarcloud.io/dashboard?id=SonarCloud.SkillsFundingAgency.dfc-personalisation-domain-pkg-netcore) | https://www.nuget.org/packages/DFC.Personalisation.Domain/

#### dfc-personalisation-common-ui-pkg-netcore

| [Branch](https://github.com/SkillsFundingAgency/dfc-personalisation-common-ui-pkg-netcore) | [Build](https://sfa-gov-uk.visualstudio.com/Digital%20First%20Careers/_build?definitionId=1926&_a=summary) | [Sonar](https://sonarcloud.io/dashboard?branch=dev&id=SonarCloud.SkillsFundingAgency.dfc-personalisation-common-ui-pkg-netcore) | Deploy
--- | --- | --- | ---
Dev   | [![Build Status](https://sfa-gov-uk.visualstudio.com/Digital%20First%20Careers/_apis/build/status/Personalisation/dfc-personalisation-common-ui-pkg-netcore?branchName=dev)](https://sfa-gov-uk.visualstudio.com/Digital%20First%20Careers/_build/latest?definitionId=1926&branchName=dev)       | N/A | N/A
Master| [![Build Status](https://sfa-gov-uk.visualstudio.com/Digital%20First%20Careers/_apis/build/status/Personalisation/dfc-personalisation-common-ui-pkg-netcore?branchName=master)](https://sfa-gov-uk.visualstudio.com/Digital%20First%20Careers/_build/latest?definitionId=1926&branchName=master) | [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=SonarCloud.SkillsFundingAgency.dfc-personalisation-common-ui-pkg-netcore&metric=alert_status)](https://sonarcloud.io/dashboard?id=SonarCloud.SkillsFundingAgency.dfc-personalisation-common-ui-pkg-netcore) | https://www.nuget.org/packages/DFC.Personalisation.CommonUI/



## Solution Structure

The solution is structured following Clean Architecture principles: 

Domain (enterprise wide logic and types)
- dfc-personalisation-common-pkg-netcore - separate nuget package - eg. string extensions, IDateTime
- dfc-personalisation-domain-pkg-netcore - separate nuget package - eg. Skills, Occupations, ActionItems, Action Plans

Application (business logic and types)
- MatchEngine - the engine which orchestrates the service taxonomy searches and post search filtering and linking with job profile data
- Interfaces for Dysac, JobProfiles, ServiceTaxonomy

Services (Infrastructure) (all external concerns)
- Dysac
- JobProfiles
- ServiceTaxonomy

Web UI
- DFC.App.MatchSkills - composite UI - the startup project


## How to run

Need:
Microsoft Azure Cosmos DB Emulator
Postman

Clone:
DFC.Composite.Paths
DFC.Composite.Regions
DFC.Composite.Shell

Run once:
Start the Cosmos DB Emulator
Set {{PathRootUrl}} variable value to where your Paths app is running eg http://localhost:7071/api/
Set {{RegionRootUrl}} variable value to where your Paths app is running eg http://localhost:7072/api/
Run the postman project scripts in Paths and Regions apps to populate Cosmos

Create a new Cosmos container called eg. "Match Skills"
Create a collection within the container, called "UserSessionsCollection" with a partition id of "/UserSessionId"
Create a copy of appsettings-template.json called appsettings.json
Add your Cosmos emulator credentials and above container and collection details to your appsettings.json

Start Paths, Regions and Shell solutions
Start match skills app 


