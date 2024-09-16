# Unity Text Loader

Load and parse text content from file for display in Unity.

## Usage

### Unity Editor

Simply add the PageLoader script to your event object and adjust the settings as needed

| Script: PageLoader |
| -- |

| Variable | Type | Purpose |
| -- | -- | -- |
| Canvas Parent | GameObject | The Unity Canvas game object in the scene. |
| Text Display Tag | string | The Unity Tag given to all displayed text game objects. |
| Data Folder | string | The folder containing XML files (relative to the `Resources` asset folder). |
| File | string | The XML file to load (relative to the Data Folder). |
| Language | string | The language code of the translation to select from the XML file.

### Unity Script

Read XML pages using the `PageReader` class.

```c#
string resourcePath /* relative to 'Assets/Resources/' */ = 'file.xml';
string lang = "en";
XmlLoad loadedXmlData = PageReader.ReadXmlFile(loadedAsset, lang);
List<Page> pages = loadedXmlData.GetPages();
foreach (Page page in pages) { Debug.Log(page.Title); }
```

## Content

### XML files

Page content is stored in XML files.
These XML files must be validated by the schema given in [Schema.xml](Schema.xml).

| XML Schema: Pages |
| -- |

| Element | Parent | Allowed Children | Attributes | Purpose |
| -- | -- | -- | -- | -- |
| `page` | (root) | `title`, `element` | *none* |  Denotes a page containing a title and elements. |
| `title` | `page` | `translate`, `default` | *none* | The title of a page. |
| `description` | `page` | `translate`, `default` | *none* | The description of a page. |
| `keywords` | `page` | `translate`, `default` | *none* | The comma-separated keywords for a page. |
| `element` | `page` | `translate`, `default` | `x`, `y`, `width`, `height`, `fontSize` | An element to be placed on the page. |
| `img` | `page`, `element` | *void* | `src`, `x`, `y`, `scale` | An image to be loaded onto the page. |
| `translate` | `title`, `element` | Free HTML | `lang` (required) | Content for a specific language.
| `default` | `title`, `element` | Free HTML | *none* | Content for when a language is missing, or content for all applicable languages.
| Free HTML | `translate`, `default` | Free HTML | *depends* | A limited subset of HTML elements. [See below](#free-html).

#### Free HTML

As well as plain text, the following pseudo-HTML tags are allowed inside "Free HTML" elements:

| Element | Effect | Allowed Attributes |
| -- | -- | -- |
| `<br/>` | Newline | *none* |
| `<hr/>` | Horizontal rule | *none* |
| `<b>` | Bold | *none* |
| `<i>` | Italic | *none* |
| `<h1>`...`<h6>` | Headings | *none* |
| `<font>` | Change font style | `color={string}`<br>`size={int}` |
| `<link>` | Internal hyperlinking | `to={file.ext}` |
| `<a>` | External hyperlinking | `href={url}` |

#### Example

```xml
<?xml version="1.0" encoding="UTF-8"?>
<page
    xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
    xsi:noNamespaceSchemaLocation="https://github.com/Bloonspedia/UnityTextLoader/raw/1.4.0/Schema.xml"
>
    <title>
        <translate lang="en">
            Page Name
        </translate>
        <translate lang="de">
            Seitenname
        </translate>
    </title>
    <description>
        <translate lang="en">
            The best page!
        </translate>
        <translate lang="de">
            Die beste Seite!
        </translate>
    </description>
    <keywords>
        <default>
            example,best,page
        </default>
    </keywords>

    <img src="img.png" x="100" y="200" scale="1.5" />

    <element x="0" y="100" width="500" height="100" fontSize="20">
        <default>
            Lorem ipsum
        </default>
    </element>
    <element x="0" y="-300" width="500" height="100" fontSize="20">
        <translate lang="en">
            Content goes <b>here</b>!
        </translate>
        <translate lang="de">
            Inhalt kommt <b>hierher</b>!
        </translate>
    </element>
</page>
```

These XML files must be placed inside a `Resources` folder inside the `Assets` folder, at any depth (i.e., matching the pattern `/Assets/**/Resources/**/`).
