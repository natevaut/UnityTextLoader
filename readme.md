# Unity Text Loader

Load and parse text content from file for display in Unity.

## Usage

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

Page content is stored in XML files. Example:
```xml
<?xml version="1.0" encoding="UTF-8"?>
<page>
    <title>
        <translate lang="en">
            Page Name
        </translate>
        <translate lang="de">
            Seitenname
        </translate>
    </title>
    <element x="0" y="100" width="500" height="100" fontSize="20">
        <!-- No translation tags = all languages will use exactly this content -->
        Lorem ipsum
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

### Formatting

The UnityTextLoader supports the following formatting tags:

| XML Tag | Effect | Allowed Attributes |
| -- | -- | -- |
| `<b></b>` | Bold | *none* |
| `<i></i>` | Italic | *none* |
| `<br/>` | Newline | *none* |
| `<hr/>` | Horizontal rule | *none* |
| `<font>` | Change font style | `color={string}`<br>`size={int}` |
| `<link>` | Internal hyperlinking | `to={file.ext}` |
| `<a>` | External hyperlinking | `href={url}` |
