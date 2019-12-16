# DFC.App.MatchSkills

ASP.Net Core 3 applet which plugs in to the Composite-UI architecture for "Match Your Skills to a Career".

For details on the Composite-UI see [https://github.com/SkillsFundingAgency/dfc-composite-shell](https://github.com/SkillsFundingAgency/dfc-composite-shell)


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
- DFC.App.MatchSkills.WebUI - composite UI - the startup project


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

Start Paths, Regions and Shell solutions
Start match skills app 


