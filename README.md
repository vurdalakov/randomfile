# RandomFile

#### Overview

**RandomFile** is a command-line C# program that generates a file with given length and random content. 

This tool is part of [`dostools` collection](https://github.com/vurdalakov/dostools). 

Distributed under the [MIT license](http://opensource.org/licenses/MIT).

This project has been automatically exported from [Google Code](http://code.google.com/p/randomfile/).

#### Command line syntax

```
randomfile file_name file_size [seed] [/ascii] [/sha1]
```

`file_name` and `file_size` parameters are mandatory. They define required file name and file size.

File size can be defined in bytes, kilobytes (number followed by `K` or `KB`), megabytes (number followed by `M` or `MB`) or gigabytes (number followed by `G` or `GB`). It is assumed that kilobyte equals to 1,024 bytes, megabyte equals to 1,048,576 bytes and gigabyte equals to 1,073,741,824 bytes.

By default files are filled with a pseudo-random byte sequence using the [System.Random](http://msdn.microsoft.com/en-us/library/system.random.aspx) class. To generate files with the same content, specify optional `seed` parameter.

The following options are supported:

* `/ascii` - generate files that consist only of ASCII characters (32-127).
* `/sha1` - calculate SHA-1 file hash.

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

#### Version history  

###### Version 1.05 (01.03.2012)

* Optionally generates ASCII files, that consist only of ASCII characters 32-127.
* Command-line syntax has slightly changed.

###### Version 1.03 (22.02.2012)

* Calculates [SHA-1 hash](http://code.google.com/p/freecodecollection/wiki/bighash) of the generated file.

###### Version 1.02 (27.01.2011)

* First public version.
