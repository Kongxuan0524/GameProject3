:start
@echo off
color 0A
echo. ╔═════════════════╗
echo. ║  请选择要进行的操作，然后按回车  ║
echo. ║═════════════════║
echo. ║                                  ║
echo. ║     1.启动全部服务器(DEBUG)      ║
echo. ║                                  ║
echo. ║     2.启动全部服务器(RELEASE)    ║
echo. ║                                  ║
echo. ║     3.关闭全部服务器             ║
echo. ║                                  ║
echo. ║     4.启动测试客户端             ║
echo. ║                                  ║
echo. ║     5.清除屏幕                   ║
echo. ║                                  ║
echo. ║     6.启动压力测试工具           ║
echo. ║                                  ║
echo. ║     7.退出命令窗口               ║
echo. ╚═════════════════╝
echo.             
set DebugDir=%cd%\Src\Debug\
set ReleaseDir=%cd%\Src\Release\
set ClientDir=%cd%\Client\TestClient\Debug\
set PressDir=%cd%\Client\\PressureTest\Debug\
:cho
set choice=
set /p choice=          请选择:
IF NOT "%choice%"=="" SET choice=%choice:~0,1%
if /i "%choice%"=="1" start /D %DebugDir% /MIN %DebugDir%LoginServer.exe & start /D %DebugDir% /MIN %DebugDir%ProxyServer.exe &  start /D %DebugDir% /MIN %DebugDir%DBServer.exe &  start /D %DebugDir% /MIN %DebugDir%GameServer.exe &  start /D %DebugDir% /MIN%DebugDir%LogServer.exe &  start /D %DebugDir% /MIN %DebugDir%LogicServer.exe & start /D %DebugDir% /MIN %DebugDir%AccountServer.exe
if /i "%choice%"=="2" start /D %ReleaseDir% /MIN %ReleaseDir%LoginServer.exe & start /D %ReleaseDir% /MIN %ReleaseDir%ProxyServer.exe & start /D %ReleaseDir% /MIN %ReleaseDir%DBServer.exe & start /D %ReleaseDir% /MIN %ReleaseDir%GameServer.exe & start /D %ReleaseDir% /MIN %ReleaseDir%LogServer.exe & start /D %ReleaseDir% /MIN %ReleaseDir%LogicServer.exe & start /D %ReleaseDir% /MIN %ReleaseDir%AccountServer.exe
if /i "%choice%"=="3" taskkill /im LoginServer.exe & taskkill /im ProxyServer.exe & taskkill /im DBServer.exe & taskkill /im GameServer.exe & taskkill /im LogServer.exe & taskkill /im LogicServer.exe & taskkill /im TestClient.exe & taskkill /im PressureTest.exe & taskkill /im AccountServer.exe
if /i "%choice%"=="4" start /D %ClientDir% %ClientDir%TestClient.exe
if /i "%choice%"=="5" cls & goto start
if /i "%choice%"=="6" start /D %PressDir% %PressDir%PressureTest.exe
if /i "%choice%"=="7" exit
if /i "%choice%"=="8" type %DebugDir%ServerCfg.ini
echo.
goto cho


