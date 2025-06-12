
TEST_PATH=$(HOME)/git/other/writing-a-c-compiler-tests
DRIVER_PATH=$(HOME)/git/dswisher/writing-c-compiler/scripts/swish-cc.sh


.PHONY: main1 lex1 codegen1 chapter1

main: chapter1


lex1:
	@echo "* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *"
	@echo "* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *"
	cd $(TEST_PATH) && \
	./test_compiler $(DRIVER_PATH) --chapter 1 --stage lex

parse1:
	@echo "* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *"
	@echo "* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *"
	cd $(TEST_PATH) && \
	./test_compiler $(DRIVER_PATH) --chapter 1 --stage parse

codegen1:
	@echo "* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *"
	@echo "* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *"
	cd $(TEST_PATH) && \
	./test_compiler $(DRIVER_PATH) --chapter 1 --stage codegen

chapter1:
	@echo "* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *"
	@echo "* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *"
	cd $(TEST_PATH) && \
	./test_compiler $(DRIVER_PATH) --chapter 1

