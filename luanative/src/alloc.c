#include <stdlib.h>
#include "lua.h"

void * defaultLuaAlloc (void *ud, void *ptr, size_t osize, size_t nsize) {
	if (nsize == 0) {
		free(ptr);
		return NULL;
	}
	return realloc(ptr, nsize);
}

LUA_API lua_Alloc lua_getDefaultAlloc() {
	return defaultLuaAlloc;
}
