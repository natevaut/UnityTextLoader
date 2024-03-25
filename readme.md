# Unity Text Loader

***Proof of concept***

----

Load and parse text content from file for display in Unity.

## Syntax

Formatting syntax is based on Markdown while link formatting is inspired by Wikitext.

| Formatting | Code | Display |
| :- | :- | :- |
| Bold | `**bold**` | **bold** |
| Italic | `*italic*` | *italic* |
| Underline | `__lined__` | <ins>lined</ins> |
| Strikethrough | `~~stricken~~` | <s>stricken</s> |
| Heading 1 | `# Heading 1` | <h1>Heading 1</h1> |
| Heading 2 | `## Heading 2` | <h2>Heading 2</h2> |
| Heading 3 | `### Heading 3` | <h3>Heading 3</h3> |
| Heading 4 | `#### Heading 4` | <h4>Heading 4</h4> |
| Internal Link | `[[link]]` | <a href="link">link</a> |
| Internal Link | `[[link`&vert;`Display]]` | <a href="link">Display</a> |
| External Link | `[http://link Display]` | <a href="http://link">Display</a> |
| Bullet Item | `- item` | &nbsp; &bull; item |
| Numbered Item | `1. item` | &nbsp; 1. item |
