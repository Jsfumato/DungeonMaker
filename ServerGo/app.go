package main

import (
	"bufio"
	"fmt"
	"os"

	"github.com/yuin/gopher-lua"
)

func main() {

	err := loadConfig("Config.lua")
	if err != nil {
		fmt.Println("[ERROR] cannot load config file")
		os.Exit(1)
	}

	mainServer := NewMainServer()
	err = mainServer.Listen()

	if err != nil {
		fmt.Println("Could not serve")
		os.Exit(1)
	}

}

func loadConfig(fileName string) error {
	// 
	lua.RegistrySize = 1024 * 20
	lua.CallStackSize = 1024

	//
	// L := lua.NewState(lua.Options{SkipOpenLibs: true})
	L := lua.NewState()
	defer L.Close()

	err := L.DoFile(fileName)
	if err != nil {
		panic(err)
		return err
	}

	return nil
}

// CompileLua reads the passed lua file from disk and compiles it.
func CompileLua(filePath string) (*lua.FunctionProto, error) {
    file, err := os.Open(filePath)
    defer file.Close()
    if err != nil {
        return nil, err
    }
    reader := bufio.NewReader(file)
    chunk, err := parse.Parse(reader, filePath)
    if err != nil {
        return nil, err
    }
    proto, err := lua.Compile(chunk, filePath)
    if err != nil {
        return nil, err
    }
    return proto, nil
}