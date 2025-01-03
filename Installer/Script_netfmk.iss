; MTGM Setup Script
; by Couitchy

[Setup]
AppName=Magic The Gathering Manager
AppVerName=Magic The Gathering Manager 1.0
AppPublisher=Couitchy Corp.
AppPublisherURL=http://couitchy.free.fr
AppSupportURL=http://couitchy.free.fr
AppUpdatesURL=http://couitchy.free.fr
DefaultDirName={pf}\Magic The Gathering Manager
DefaultGroupName=Magic The Gathering Manager
AllowNoIcons=yes
LicenseFile=..\Ressources\Licence.txt
OutputDir=..\Installer
OutputBaseFilename=MTGM_setup
Compression=lzma
SolidCompression=yes

[Languages]
Name: "eng"; MessagesFile: "compiler:Default.isl"
Name: "fre"; MessagesFile: "compiler:Languages\French.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "..\MTGM\_bin\Release-x86\Magic The Gathering Manager.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\MTGM\_bin\Release-x86\Updater.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\MTGM\_bin\Release-x86\Ressources\*.*"; DestDir: "{app}\Ressources"; Flags: ignoreversion
Source: "..\MTGM\_bin\Release-x86\Cartes\Magic DB.mdb"; DestDir: "{app}\Cartes"; Flags: ignoreversion skipifsourcedoesntexist
Source: "..\MTGM\_bin\Release-x86\MTGM.pdf"; DestDir: "{app}"; Flags: ignoreversion skipifsourcedoesntexist
Source: "..\MTGM\_bin\Release-x86\Historique.txt"; DestDir: "{app}"; Flags: ignoreversion skipifsourcedoesntexist
Source: "..\MTGM\_bin\Release-x86\ChartFX.Lite.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\MTGM\_bin\Release-x86\NPlot.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\MTGM\_bin\Release-x86\TreeViewMS.dll"; DestDir: "{app}"; Flags: ignoreversion
; Source: "..\MTGM\_bin\Release-x86\Win7Taskbar.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\MTGM\_bin\Release-x86\ICSharpCode.SharpZipLib.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\MTGM\_bin\Release-x86\SandBar.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\MTGM\_bin\Release-x86\SourceGrid2.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\MTGM\_bin\Release-x86\SourceLibrary.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Libs\isxdl.dll"; Flags: dontcopy

[UninstallDelete]
Type: filesandordirs; Name: "{app}"
Type: filesandordirs; Name: "{code:GetVirtualPath}"

[Icons]
Name: "{group}\Magic The Gathering Manager"; Filename: "{app}\Magic The Gathering Manager.exe"; WorkingDir: "{app}"
Name: "{group}\{cm:UninstallProgram,Magic The Gathering Manager}"; Filename: "{uninstallexe}"
Name: "{userdesktop}\Magic The Gathering Manager"; Filename: "{app}\Magic The Gathering Manager.exe"; WorkingDir: "{app}"; Tasks: desktopicon

[_ISTool]
EnableISX=true

[Code]
var
dotnetRedistPath: string;
downloadNeeded: boolean;
dotNetNeeded: boolean;
memoDependenciesNeeded: string;

procedure isxdl_AddFile(URL, Filename: PChar);
external 'isxdl_AddFile@files:isxdl.dll stdcall';
function isxdl_DownloadFiles(hWnd: Integer): Integer;
external 'isxdl_DownloadFiles@files:isxdl.dll stdcall';
function isxdl_SetOption(Option, Value: PChar): Integer;
external 'isxdl_SetOption@files:isxdl.dll stdcall';
function isxdl_IsConnected: Integer;
external 'isxdl_IsConnected@files:isxdl.dll stdcall';

const
dotnetRedistURL = 'http://download.microsoft.com/download/2/0/E/20E90413-712F-438C-988E-FDAA79A8AC3D/dotnetfx35.exe';

function InitializeSetup(): Boolean;
begin
  Result := true;
  dotNetNeeded := false;
  // Check for required netfx installation
  if (not RegKeyExists(HKLM, 'Software\Microsoft\.NETFramework\v3.0')) then begin
    dotNetNeeded := true;
    if (not IsAdminLoggedOn()) then begin
      MsgBox('Magic The Gathering Manager n�cessite l''installation du Microsoft .NET Framework 3.5 sur cet ordinateur en tant qu''Administrateur.', mbInformation, MB_OK);
      Result := false;
    end else begin
      memoDependenciesNeeded := memoDependenciesNeeded + '  Microsoft .NET Framework 3.5' #13;
      dotnetRedistPath := ExpandConstant('{src}\dotnetfx.exe');
      if not FileExists(dotnetRedistPath) then begin
        dotnetRedistPath := ExpandConstant('{tmp}\dotnetfx.exe');
        if not FileExists(dotnetRedistPath) then begin
          isxdl_AddFile(dotnetRedistURL, dotnetRedistPath);
          downloadNeeded := true;
        end;
      end;
      SetIniString('install', 'dotnetRedist', dotnetRedistPath, ExpandConstant('{tmp}\dep.ini'));
    end;
  end;
end;

function NextButtonClick(CurPage: Integer): Boolean;
var
hWnd: Integer;
ResultCode: Integer;
begin
  Result := true;
  if CurPage = wpReady then begin
    hWnd := StrToInt(ExpandConstant('{wizardhwnd}'));
    // don't try to init isxdl if it's not needed because it will error on < ie 3
    if downloadNeeded then begin
      if isxdl_IsConnected = 1 then begin
        isxdl_SetOption('label', 'T�l�chargement du Microsoft .NET Framework 3.5');
        isxdl_SetOption('description', 'Merci de patienter pendant que l''installeur t�l�charge des fichiers suppl�mentaires n�cessaires.');
        if isxdl_DownloadFiles(hWnd) = 0 then begin
          MsgBox('Magic The Gathering Manager n�cessite la pr�sence du Microsoft .NET Framework 3.5.' + Chr(13) + 'Si votre ordinateur est connect� � Internet, vous pouvez essayer de d�sactiver temporairement votre pare-feu afin de d�buter automatiquement le t�l�chargement.' + Chr(13) + 'Vous pouvez sinon installer manuellement le fichier requis, disponible � l''adresse : ' + dotnetRedistURL + '.' , mbError, MB_OK);
          Result := false;
        end;
      end else begin
        MsgBox('Magic The Gathering Manager n�cessite la pr�sence du Microsoft .NET Framework 3.5.' + Chr(13) + 'Si votre ordinateur est connect� � Internet, vous pouvez essayer de d�sactiver temporairement votre pare-feu afin de d�buter automatiquement le t�l�chargement.' + Chr(13) + 'Vous pouvez sinon installer manuellement le fichier requis, disponible � l''adresse : ' + dotnetRedistURL + '.' , mbError, MB_OK);
        Result := false;
      end;
    end;
    if (Result = true) and (dotNetNeeded = true) then begin
      if Exec(ExpandConstant(dotnetRedistPath), '', '', SW_SHOW, ewWaitUntilTerminated, ResultCode) then begin
        // handle success if necessary; ResultCode contains the exit code
        if not (ResultCode = 0) then begin
          Result := false;
        end;
      end else begin
        // handle failure if necessary; ResultCode contains the error code
        Result := false;
      end;
    end;
  end;
end;

function UpdateReadyMemo(Space, NewLine, MemoUserInfoInfo, MemoDirInfo, MemoTypeInfo, MemoComponentsInfo, MemoGroupInfo, MemoTasksInfo: String): String;
var
s: string;
begin
  if memoDependenciesNeeded <> '' then s := s + 'Module(s) compl�mentaire(s) :' + NewLine + memoDependenciesNeeded + NewLine;
  s := s + MemoDirInfo + NewLine + NewLine;
  Result := s
end;

function GetVirtualPath(Param: String) : String;
var
s: string;
begin
  s := ExpandConstant('{app}');
  Delete(s, 1, 2);
  s := ExpandConstant('{userappdata}') + '\Local\VirtualStore' + s;
  StringChangeEx(s, '\Roaming', '', True);
  Result := s;
end;
