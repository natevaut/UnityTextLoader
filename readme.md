# Unity Text Loader

Load and parse text content from file for display in Unity.

## Usage

Simply add the PageLoader script to your event object and adjust the settings as needed

| Script: PageLoader |
| -- |

| Variable | Type | Purpose |
| -- | -- | -- |
| Canvas Parent | GameObject | Unity Canvas Object |
| Data Folder | string | Folder to file XML files in |
| Filename | string | The XML file to load |

Page content is stored in XML files. Example:
```xml
<?xml version="1.0" encoding="UTF-8"?>
<page title="Lorem Ipsum">
    <element x="0" y="100" width="500" height="100" fontSize="20">
        Lorem ipsum
    </element>
</page>
```

### Formatting

The UnityTextLoader supports the following formatting tags:

| XML Tag | Effect | Allowed Attributes |
| -- | -- | -- |
| `<b></b>` | Bold | *none* |
| `<i></i>` | Italic | *none* |
| `<br/>` | Newline | *none* |
| `<hr/>` | Horizontal rule | *none* |
| `<font>` | Change font style | `color={string}`<br>`size={int}` |
