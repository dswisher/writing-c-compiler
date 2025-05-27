
.PHONY: all clean

all: return_2


return_2.s: return_2.c
	# gcc -S -O -fno-asynchronous-unwind-tables -fcf-protection=none return_2.c
	# gcc-14 -S -O -arch x86_64 -fno-asynchronous-unwind-tables -fcf-protection=none return_2.c
	x86_64-elf-gcc -S -O -fno-asynchronous-unwind-tables -fcf-protection=none return_2.c

return_2: return_2.s
	x86_64-elf-gcc return_2.s -o return_2


clean:
	rm -f return_2.s return_2

