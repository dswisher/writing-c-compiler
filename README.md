# Intro

This repo contains my work as I go through the book [Writing a C Compiler](https://nostarch.com/writing-c-compiler), by Nora Sandler.


# Links

* The book has a [resources page](https://norasandler.com/book/)
* The [test suite](https://github.com/nlsandler/writing-a-c-compiler-tests/) for the book


# M1 Mac Notes

Since I am working on an M1 Mac, I need to switch to a shell running under Rosetta 2, so that it thinks I'm on Intel:

    arch -x86_64 zsh

Another alternative is to use the x64 tools:

    brew install x86_64-elf-gcc

That makes the binary, `x86_64-elf-gcc`, available for use, in place of gcc.
While that generates x64 assembly, I was not able to link the program:

    x86_64-elf-ld: cannot find crt0.o: No such file or directory

There is probably a way around that, but it eludes me at the moment.

