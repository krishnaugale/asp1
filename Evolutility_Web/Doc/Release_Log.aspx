<%@ Page AutoEventWireup="true" CodeFile="Doc.aspx.cs" CodeFileBaseClass="BasePage"
    Inherits="EvoDoc" Language="C#" MasterPageFile="zmDoc.master" 
Menus="more"
SubMenuID="400"
    Meta_Description="Evolutility release log."  
    Title="Evolutility : Release log" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server"> 
 
<h1>Evolutility releases log</h1>
 
<pre>  

Evolutility Patch - version 4.0.1 - 07/14/2012

Evolutility Patch 4.0.1 provides small UI improvements (mostly for IE).

To install the patch, replace on your web site the files "Evol.css", "Evolutility.js" and "Evolutility.dll" by the new version provided with this download.
Also, replace the stored procedure "EvoSP_PagedItem" by the one provided in the new SQL script (thanks to Arun Tech for his contribution).

Enhancements:
Changed JSON export format to pass JSONlint validation.

Bug fix: 
3544302 - Panel Labels not displaying correctly w/ IE.

More about Evolutility lightweight framework for heavy lifting at http://www.evolutility.org/

#################################################################

Many THANKS for their translation:
- Catalan by Oscar Benadi.
- Chinese (simplified) by Sam Zhou.
- Danish by Henrik Holm.
- English by Olivier Giulieri.
- French by Eddy Boels.
- German by Joachim Seidel.
- Hindi by P.K.Agarwal.
- Italian by Pier Giuseppe Meo.
- Japanese by Kazue Watanabe.
- Persian (Farsi) by Sohail Abbasi.
- Portuguese by Gilberto Botaro.
- Romanian by Cosmin Munteanu.
- Spanish by Gilberto Botaro.
- Turkish by Davut Engin.

and also for their code or contribution to: 
- Terry Kernan for helping with cross-browser issues
- Mark James for his wonderful icon library (http://www.famfamfam.com)
- Julian Robichaux for his Date Picker widget
- All the people who gave me feedback and encouragements

http://www.evolutility.org

#################################################################

Evolutility - version 4.0 - 12/12/2011

Evolutility 4.0 brings charting capabilities and a brand new look.

Enhancements:
New look with rounded corners.
Charting capabilities.
Updated TinyMCE (for rich text) to version 3.4.7 and include more features.

Bug fixes:
3444826 - Navigation icons disappears with right-to-left languages.
3441315 - Required fields of type "Rich Text" don't validate properly
3177949 - Toolbar labels appear on top of icons.
2882288 - International Date Format. Thanks to Gurmit Singh for contributing the fix.

More about Evolutility lightweight framework for heavy lifting at http://www.evolutility.org/

#################################################################

Evolutility Patch - version 3.6.2 - 11/06/2011

Evolutility Patch 3.6.2 fixes the bug #3177949 where on some browsers the toolbar labels appearing 
on top of icons.

To install the patch, replace on your web site the files "Evol.css" and "Evol-green.css" 
by the new version provided with this download.

More about Evolutility lightweight framework for heavy lifting at http://www.evolutility.org/

#################################################################

Evolutility Patch - version 3.6.1 - 10/18/2011

Evolutility Patch 3.6.1 fixes the international date formats bug #2882288. 

To install the patch, replace the file Evodico/JS/EvoDates.js by the new version provided 
with this download. 

More about Evolutility lightweight framework for heavy lifting at http://www.evolutility.org/

#################################################################

Evolutility - version 3.6 - 06/17/2011

Evolutility 3.5 is a maintenance release with bug fixes and improvements to EvoDico Wizards. 
EvoDico (to use the database instead of XML) is included with Evolutility.

Bug fixes:
3099581 - Add code for "left bar" to the download
3064489 - Hide panel sometimes doesn't work on the last panel w/ tabs.
3088972 - In Export page, "show all" link is there w/ all fields.
3064497 - Icon repeated in first fields of every tab.
3059670 - Warning when compiling ExportWizard.

More about Evolutility lightweight framework for heavy lifting at http://www.evolutility.org/

#################################################################

Evolutility - version 3.5 - 11/01/2010

Evolutility 3.5 is a mmaintenance release with bug fixes and improvements to EvoDico Wizards. 
EvoDico (to use the database instead of XML) is included with Evolutility.

Bug fixes:
3099581 - Add code for "left bar" to the download
3064489 - Hide panel sometimes doesn't work on the last panel w/ tabs.
3088972 - In Export page, "show all" link is there w/ all fields.
3064497 - Icon repeated in first fields of every tab.
3059670 - Warning when compiling ExportWizard.

More about Evolutility lightweight framework for heavy lifting at http://www.evolutility.org/

#################################################################

Evolutility - version 3.4 - 06/22/2010

Evolutility 3.6 is a radical change in look and the addition of Persian (Farsi) localization.

Bug fixes:
3138581	- Export more than 20 records 

More about Evolutility lightweight framework for heavy lifting at http://www.evolutility.org/

#################################################################

Evolutility - version 3.3 - 05/10/2010

Evolutility 3.3 is getting back to real lightweight by replacing the Rich Text Editor CKEditor 
with TinyMCE.
It also provides a few bug fixes. 

Bug fixes:
2997093 - Dependant LOV problem w/ special characters.
2997976 - DatePicker problem in Hindi, German, and Turkish.
2997135 (regression) - can't update other user's records with Multiple_Users_Collaboration.
2997138 (regression) - Problem sorting columns with LOV records.

More about Evolutility lightweight framework for heavy lifting at http://www.evolutility.org/

#################################################################

Evolutility - version 3.2 - 04/29/2010

Evolutility 3.2 adds a new web form for "Mass Update". 
It also includes AJAX and CSS improvements (mostly for Google Chrome browser), 
as well as bug fixes.

Bug fixes:
2993992 - Dependant fields do not work properly w/ Chrome.
2803819 - Icon for popup not showing w/ Chrome.
2991576 - UserID not saved when using "Multiple_Users_Collaboration".

More about Evolutility lightweight framework for heavy lifting at http://www.evolutility.org/

#################################################################

Evolutility - version 3.1 - 02/16/2010

Evolutility 3.1 adds JSON (JavaScript Object Notation) as an export format, easier access to 
EvoDico tables, and upgrade to CKeditor 3.1.

Enhancements:
2946845 - Upgrade of Rich Text Format editor to CKeditor 3.1.
2952629	- Add JSON as an export format.
2952379 - Add easy way to switch between DBs for data and metadata.

Bug fixes:
2943490 - "search" and "selections" should be disabled if not loggedin.
2913901 - SQL Scripts Not in Download.

Note: EvoDico 3.1 is a separate download.

More about Evolutility lightweight framework for heavy lifting at http://www.evolutility.org/

#################################################################

Evolutility - version 3.0 - 09/17/2009

Evolutility 3.0 is a major release of Evolutility open source framework for CRUD. 

After SQL Server, it now also provides support for MySQL database. 
It adds Rich Text Format with the integration of FCKeditor.
It also includes new localization in German, Hindi, and Turkish; now making Evolutility 
available in 13 languages. 
 
Enhancements:
2824630 - Support for MySQL
2819222	- Rich Text Editor with the integration of FCKeditor
2823734 - Multi-Select Listboxes for advanced search

Bug fixes:
2826299 - Date field saved empty in Spanish language
2830694 - Query description in adv. search uses ID instead of values.
2817275 - "Back to search results" link poorly positioned.

More about Evolutility lightweight framework for heavy lifting at http://www.evolutility.org/

#################################################################

Evolutility - version 3.0 Beta.2 - 09/01/2009

Evolutility 3.0 will be a major release providing localization in German and Hindi, 
as well as new features, and bug fixes. 

Enhancements:
2819222	- Rich Text Editor
2823734 - Multi-Select Listboxes for advanced search

Bug fixes:
2826299 - Date field saved empty in Spanish language
2830694 - Query description in adv. search uses ID instead of values.
2817275 - "Back to search results" link poorly positioned.

More about Evolutility lightweight framework for heavy lifting at http://www.evolutility.org/

#################################################################

Evolutility - version 3.0 Beta.1 - 08/14/2009

Evolutility 3.0 will be a major release providing new features, and bug fixes. 

Enhancements:
2819222	- Rich Text Editor
2823734 - Multi-Select Listboxes for advanced search

Bug fixes:
2826299 - Date field saved empty in Spanish language
2830694 - Query description in adv. search uses ID instead of values.
2817275 - "Back to search results" link poorly positioned.

More about Evolutility lightweight framework for heavy lifting at http://www.evolutility.org/

#################################################################

Evolutility - version 2.6.1 - 07/08/09

Evolutility version 2.6.1 is a minor release providing enhancements to the Chinese (simplified) 
translation, and bug fixes. 

Bug fixes:
2816741 - Javascript sometimes erroring on back button
2817277 - Missing files in Evolutility_2.6.zip

More about Evolutility lightweight framework for heavy lifting at http://www.evolutility.org/

#################################################################

Evolutility version 2.6 - 06/29/2009 

With Version 2.6, Evolutility CRUD framework is moving to AJAX, and adding support for Chinese 
(simplified) language. 

AJAX will provide more dynamic pages and better performance. Search, Advanced search, and 
Selections forms are now partial refreshes of the page. More will AJAX will follow 
in the next few months.

With the latest addition of the simplified Chinese translation, Evolutility now supports 
10 languages: 
Catalan, Chinese (Simplified), Danish, English, French, Italian, Japanese, Portuguese, Romanian, 
and Spanish. 

Live demo at http://evolutility.org/demo/demo_addressbook.aspx?LNG=ZH

Bug fixes:
2813159 - Query description missing field names in search result


#################################################################

Evolutility version 2.5.4 - 05/04/2009 

This is a minor release for Italian language support and small bug fixes.

The release also includes Evolutility Dictionary and a mapping of the Chinook sample database.

#################################################################

Evolutility version 2.5.3 - 03/29/2009 

Minor release for 2 bug fixes.
- Validation with translation bug.
- Last column in details skipped bug.

#################################################################

Evolutility version 2.5.2 - 03/16/2009 

Minor release for 2 really bad regression bugs.
2688338 - Regression w/ date fields  
2688274 - Regression w/ comments  

#################################################################

Evolutility version 2.5.1 - 03/12/2009 

This release focuses on usability: 

Holistic improvements:
- Distinction between primary and secondary action buttons
- Added CSS classes for different messages severities (background-color and icons)
- Added visual flag to links with popups
- Added new property "DBAllowLogout" to show or hide the "Logout" button on the toolbar 
(useful when bypassing Evolutility authentication)
- Distinction between read-only fields and user comments
- Icons improvements and checkmark icons in sprite
- Improvements in generated HTML toward better web standards.
- Partial support for DocType XHTML strict and transitional.
- Support for right-to-left justified languages.
- Added localizations to Danish and Romanian. 

Improvements to the Edit form:
- Added new property for Tabs orientation (called "TabPosition" b/c Left or Right will be 
supported in the future).
- icons for actions on details grid: Add Row and Delete Row.
- flag required fields in panel-details (no validation yet)

Improvements to the Search form:
- Labels now left aligned for faster scanning.
- Added "is empty" and "is not empty" operators to advanced search (for fields not required).

Improvements to the Export form:
- moved "Download" button, and use discoverable list of fields

Bug fixes (mostly in EvoDico): 
2638873 - Duplicate namespace attribute in generated XML. 
2638877 - No need for Primary key as explicit field.
2639754 - Removed "icon" and "dbcolumnicon" from form in SP EvoDico_Form_Get.
2639754 - Removed "ppos" from "panel", removed "typeid" and "fpos" from "field" on generated XML.

Technical details (new CSS class names):
- Distinction between primary and secondary actions: "Button" and "Button2"
- Different messages severities: "MsgInfo", "MsgError", "MsgWarn", "MsgDel", and "MsgHelp"
- Visual flag to links with popups: "ExtWeb"
- Distinction between read-only fields and user comments: "FieldReadOnly" and "FieldComments"
- Checkmark icons in sprite: "CheckGreen", "CheckBlue", "CheckRed", and "CheckBlack" 
- Tabs orientation: "TabLeft"
- icons for details: "AddRow", "DelRow"
- Labels now left aligned: "SearchLabel"


More information on Evolutility lightweight CRUD framework at http://www.evolutility.org


#################################################################

Evolutility Documentation and Translation Kit - 02/26/2009 

We are pleased to announce the release of Evolutility Documentation and Translation Kit.

The documentation comes as a set of 35 HTML pages with many screenshots. 
It covers topics of Evolutility CRUD framework like Web control, Database dictionary, Authentication, 
Custom validation, Skins, Meta-model, and more.
 
In the last 3 months, our users translated Evolutility to Spanish, Japanese, Portuguese, and Catalan 
(in addition to English and French).
The translation kit will make it easier for anyone to help us support more languages in our next releases.  	 
	 
Evolutility Documentation and Translation Kit are available for download at 
 - SourceForge: http://sourceforge.net/project/showfiles.php?group_id=225915 
	
or as live on Evolutility.org web site
 - Documentation: http://www.evolutility.org/doc/doc.aspx
 - Translation kit: http://www.evolutility.org/more/Get_Involved_Translation.aspx	

Evolutility project welcomes contributions... in code, translations, or simply feedback.
	
More information on Evolutility lightweight CRUD framework at http://www.evolutility.org

#################################################################

Evolutility version 2.5 - 02/10/2009 

This is an important release adding many new features, a new .net assembly, and some AJAX 
(for dependent dropdowns and contextual help).

This release includes: 
- Support for Catalan language.
- Better Master-details w/ multiple details and improved client fields validation
- Dependent dropdowns (Beta)
- User help
- Improved positioning of UI elements tabs and panels allowing a header panel above the set of tabs.
- Adding a httpHandler (for new features using AJAX)
- Improved performances and smaller footprint
- Look & feel improvements
 
Bug fixes: 
2533513 - Conditions separator not translated in search. 
2544816 - Lose selection after navigation by link. 
2533513 - Unable to have more than 1 panel-detail grid in page. 
2581893 - Checkbox adds checked="checked" outside of tag 

http://www.evolutility.org

#################################################################

Evolutility version 2.4.3 - 01/20/2009 

This is a minor release fixing toolbar issues with and IE6 and Safari.

Bug fixes: 
2523856 - Toolbar problem w/ IE6. 
2503159 - Toolbar icons truncated in Safari. 

"Release early. Release often."

http://www.evolutility.org

#################################################################

Evolutility version 2.4.2 - 01/17/2009 

This is a minor release focusing on bug fixes and optimizations.

Bug fixes: 
2503159 - Export as XML doesn't give choice of Elements or attributes. 
2422948 - Invalid results with Advanced search on date field. 

This release also includes:
- Added a new assembly Evolutility.DataServer. It is a http handler to reply to AJAX calls 
(soon to be implemented).
- Renamed field attribute "validation" by "jsvalidation".
- Added new field attributes "dependency" and "jsdependency" (for future implementation of 
AJAX dependent fields).
- Improved CSS.

http://www.evolutility.org

#################################################################

Evolutility version 2.4.1 - 01/07/2009 

This is a minor release. It adds custom validation and a few other enhancements but focuses mostly 
on bug fixes.

This release includes: 
- Custom validation with calls to external JavaScript API 
- Regular Expression validation 
- Metadata: added attribute "cssclasslabel" to Element Field 
- New CSS for green color theme
- Added CSS classes "RowInfo", "FormLogin", "FormSearch", and "FormExport". 

Bug fixes: 
2394036 - Export crash (Spanish and Portuguese versions only). 
2422948 - Export empty when PK different than "ID". 
2465681 - Incomplete description of search result. 
2481641 - Cannot add rows to an empty grid, invalid number of generated grid columns. 

The documentation is now included with the download.

http://www.evolutility.org

#################################################################

Evolutility version 2.4 - 12/05/2008 

The most important change is that the code base is now in C# instead of VB.net. This will not change 
anything for end-users but will change many things for developers interested in modifying the code 
and participating in our open source project.  
 
This release include:  
- Support fo Japanese, Spanish and Portuguese (in addition to English and French)  
- Better cross-browser support (Master-details now works in FireFox and Safari too)  
- Improved performances (and smaller generated HTML)  
- Look & feel improvements  
- Improved icon set  
 
Technical changes:  
- Change of namespace from "Evolutility.WebControls" to "Evolutility"  
- Improved DB structure for EvoDico  
- Database referencial integrity for EvoDico tables.  
 
DB dictionary changes and improvements:  
- Adding XML import wizard.  
- Adding Setup wizard.  
- Adding App Specs  

http://www.evolutility.org

#################################################################

Evolutility version 2.3.1 - 10/17/2008 

This release includes a few small bug fixes: 
- Disabling navigation on first and last record. 
- Sorting arrows for FireFox. 
... 

http://www.evolutility.org

#################################################################

Evolutility version 2.3 - 09/27/2008 

This release includes the changes: 
 
Evolutility 
- Added ability to specify the primary key column name. Now have a new XML attribute "dbcolumnpk" 
in element "data", instead of the default "ID". 
- Added new server event "CredentialChange" triggered on login and logout. 
- Fix grid header sorting arrows. 
- Changes in EvoDico structure and EvoSP_PagedItem for new parameter "dbcolumnpk". 
- Misc. small CSS & JS changes. 
- Added "technical demos" for Permissions, Localization, Navigation, and Events. 
- Better documentation. 
 
Evolutility dictionary  
- Added DB scan wizard. 
- Renamed control "EvoDicoWiz". 
- Fixed designer title bug. 
- Added "dbcolumnpk" to meta-model and EvoSP_PagedItem for new parameter.  
 
Also, we are now numbering releases of Evolutility and Evodico the same to avoid confusion. 

http://www.evolutility.org

#################################################################

Evolutility version 2.2 - 08/30/2008 

Evolutility first release in open source under Affero GPL v3 (with dual licensing).

http://www.evolutility.org

#################################################################

</pre>


</asp:Content>

