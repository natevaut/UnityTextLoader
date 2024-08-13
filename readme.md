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
| Filename | string | The XML file to load (relative to the Data Folder). |

Page content is stored in XML files. Example:
```xml
<?xml version="1.0" encoding="UTF-8"?>
<page>
    <title>Page Name</title>
    <element x="0" y="100" width="500" height="100" fontSize="20">
        Lorem ipsum
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
