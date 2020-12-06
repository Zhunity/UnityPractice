local function class(name, super)
    local cls = {
        __super = super,
        __cname = name,
    }

    setmetatable(cls, {__index = super,  __call = function(self, ...) return self.new(...) end})

    if not cls.ctor then
        cls.ctor = function(self, ...)
        end
    end

    if not cls.dtor then
        cls.dtor = function(self, ...)
        end
    end

    cls.__meta = {__index = cls}
    cls.new = function(...)
        local obj = setmetatable({}, cls.__meta)
        obj.__class = cls
        obj:ctor(...)

        local proxy = newproxy(true)
        obj.__proxy = proxy
        getmetatable(proxy).__gc = function()
            obj:dtor()
        end
        return obj
    end
    return cls
end

local Parent = class("Parent")

function Parent:ctor(index)
    self.index = index
    print(self.__cname .. "  " .. self.index)
end

function Parent:PrintHello()
    print("parent hello")
end

function Parent:PrintBye()
    print("parent bye")
end

function Parent:dtor()
    error(self.__cname .. "  " .. self.index)
end

local Child = class("Child", Parent)

function Child:PrintHello()
    print("child hello")
end

function Child:dtor()
    error(self.__cname ..  "  " .. self.index )
end




local c = Parent(11)
c = nil

local d = Child.new(12)

d:PrintHello()
d:PrintBye()
d = nil
collectgarbage("collect")