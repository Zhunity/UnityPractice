::启用命令扩展，参加setlocal /?命令
Setlocal ENABLEDELAYEDEXPANSION
set svn_path=svn://192.168.0.200/client-01/branches/alpha
set ch1=/
::复制字符串，用来截短，而不影响源字符串
set str=%svn_path%

:next
if not "%str%"=="" (
set /a num+=1
if "!str:~-1!"=="%ch1%" goto last
::比较首字符是否为要求的字符，如果是则跳出循环
set "str=%str:~0,-1%"
goto next
)
set /a num=0
::没有找到字符时，将num置零
:last
set var=%svn_path:~num%

echo 字符'%ch1%'在字符串"%str1%"中的首次出现位置为%num%  %var%
echo 输出完毕，按任意键退出&&pause>nul&&exit
