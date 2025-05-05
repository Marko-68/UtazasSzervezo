# UtazásSzervező

Az **UtazásSzervező** egy webalapú alkalmazás, amely lehetővé teszi a felhasználók számára, hogy szállásokat és repülőjáratokat keressenek, foglaljanak, valamint értékeléseket írjanak. Az adminisztrátorok külön WPF-alapú felületen kezelhetik az adatokat.
<br/>
# Adatbázis indítása:<br/>
Indítsd el a XAMPP Control Panel-t<br/>
Kapcsold be az Apache és MySQL szolgáltatásokat
# Projekt klónozása:<br/>
git clone https://github.com/Marko-68/UtazasSzervezo.git
<br/>
# Alkalmazás indítása:
Indítás előtt Buildeld a Solution-t<br/>
Konfiguráld a startup projekteket (API és UI)<br/>
Nyomd meg az F5-et a Visual Studio-ban
<br/>
# Adatbázis létrehozása:<br/>
    Update-Database -Context UtazasSzervezoDbContext -StartupProject UtazasSzervezo_UI -Project UtazasSzervezo_Library
