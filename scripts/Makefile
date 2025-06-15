
TEST_PATH=$(HOME)/git/other/writing-a-c-compiler-tests
DRIVER_PATH=$(HOME)/git/dswisher/writing-c-compiler/scripts/swish-cc.sh


.PHONY: main lex1 parse1 codegen1 chapter1 lex2 parse2 tacky2

main: chapter1


bars:
	@echo "* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *"
	@echo "* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *"


lex1: bars
	cd $(TEST_PATH) && \
	./test_compiler $(DRIVER_PATH) --chapter 1 --stage lex


parse1: bars
	cd $(TEST_PATH) && \
	./test_compiler $(DRIVER_PATH) --chapter 1 --stage parse


codegen1: bars
	cd $(TEST_PATH) && \
	./test_compiler $(DRIVER_PATH) --chapter 1 --stage codegen


chapter1: bars
	cd $(TEST_PATH) && \
	./test_compiler $(DRIVER_PATH) --chapter 1


lex2: bars
	cd $(TEST_PATH) && \
	./test_compiler $(DRIVER_PATH) --chapter 2 --stage lex

parse2: bars
	cd $(TEST_PATH) && \
	./test_compiler $(DRIVER_PATH) --chapter 2 --stage parse

tacky2: bars
	cd $(TEST_PATH) && \
	./test_compiler $(DRIVER_PATH) --chapter 2 --stage tacky

