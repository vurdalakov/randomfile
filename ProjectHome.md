**RandomFile** is a command-line C# program that generates a file with given length and random content.

<a href='http://code.google.com/p/randomfile/downloads/list'><img src='http://code.google.com/hosting/images/dl_arrow.gif' align='top' /></a> Stable **version 1.05** [is available for download](http://code.google.com/p/randomfile/downloads/list). It may need [Microsoft .NET Framework 2.0](http://www.microsoft.com/downloads/details.aspx?familyid=0856eacb-4362-4b0d-8edd-aab15c5e04f5&displaylang=en) to be installed on some old systems.

[Version History](VersionHistory.md)

This tool is part of [`dostools` collection](https://github.com/vurdalakov/dostools).

<br />

Command-line syntax:
```
    randomfile file_name file_size [seed] [/options]
```
**`file_name`** and **`file_size`** parameters are mandatory. They define required file name and file size.

File size can be defined in bytes, kilobytes (number followed by **K** or **KB**), megabytes (number followed by **M** or **MB**) or gigabytes (number followed by **G** or **GB**). It is assumed that kilobyte equals to 1,024 bytes, megabyte equals to 1,048,576 bytes and gigabyte equals to 1,073,741,824 bytes.

By default files are filled with a pseudo-random byte sequence using the [System.Random](http://msdn.microsoft.com/en-us/library/system.random.aspx) class. To generate files with the same content, specify optional **`seed`** parameter.

The following options are supported:
<ul>
<li><b><code>/ascii</code></b> - generate files that consist only of ASCII characters (32-127)</li>
<li><b><code>/sha1</code></b> - calculate SHA-1 file hash</li>
</ul>

The following examples will create 4 files with the size of 1 gigabyte and with different pseudo-random content:
```
    randomfile random1.bin 1073741824
    randomfile random2.bin 1048576K
    randomfile random3.bin 1024M
    randomfile random4.bin 1G
```
The following examples will create 3 files with the size of 2 gigabytes and with the same content:
```
    randomfile random1.bin 2097152kb 12345
    randomfile random2.bin 2048mb 12345
    randomfile random3.bin 2gb 12345
```

<br />

[![](http://randomfile.googlecode.com/svn/wiki/images/softpedia_free_award_f.gif)](http://www.softpedia.com/get/System/File-Management/RandomFile.shtml)