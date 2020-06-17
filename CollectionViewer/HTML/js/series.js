// Type of editions
var arrayClassementEdition = new Array('bases','extensions','compilations','masterpieceSeries','dueldecks','fromVault','premiumDecks','giant','spellbook','globalseries','nonStandard','introductory','nonlegal','online','promo');
var arrayLibelleClassementEdition = new Array('Editions de base','Extensions','Compilations','Masterpiece Series','Duel Decks','From The Vault','Premium & Event Decks','Two-Headed Giant','Signature Spellbook','Global Series','Non Standard Legal Sets','Introductory Sets','Non Legal For Tournament Sets','Online Exclusive Sets','Promotionnal Sets');

// List of MTGM codes
var codes_mtgm=["1E","3B","3W","4E","5E","6E","7E","8E","9E","A2","AC","AH","AJ","AK","AL","AN","AP","AQ","AT","AR","BB","BE","BG","BK","BR","BT","BZ","C0","C1","C3","C4","C5","C6","C7","C8","C9","CA","CB","CD","CF","CH","CK","CS","CT","CY","D0","D1","D2","D3","D4","D5","D6","D7","D8","D9","DA","DB","DC","DD","DG","DH","DI","DJ","DK","DL","DM","DO","DP","DQ","DR","DS","DT","DU","DV","DZ","EM","ER","ET","EX","FD","FE","FM","FR","FS","GC","GK","GN","GP","GR","GS","GW","GX","HD","HM","HP","IA","IK","IM","IN","IS","IV","JG","JN","JU","KD","KI","KT","LE","LG","LW","M1","M2","M3","M4","M5","M6","M9","M0","MA","MB","MD","ME","MG","MH","MM","MP","MR","MS","MT","MU","MV","MW","MY","NE","NP","O2","O3","O4","OD","OG","ON","OR","P1","P2","P3","PC","PG","PH","PI","PR","PS","PY","R1","R2","R3","RA","RE","RG","RI","RR","RT","RV","RX","S1","S2","SC","SD","SH","SI","SK","SL","SM","SS","ST","SV","TB","TC","TD","TE","TH","TO","TP","TS","UB","UC","UD","UG","UH","UL","UM","UN","US","UT","V1","V2","V3","V4","V5","V6","V7","V8","V9","VA","VS","WD","WE","WL","WS","WW","XL","XP","YR","ZK","ZX"];

// Structure of array
// series['MTGM_EDITION_CODE'] = new Array('EDITION_CODE','NAME_ENG','NAME_FR','NB CARDS','YEAR');
var series = new Array();
// Core sets
series['AL'] = new Array('LEA','Alpha','','295','1993');
series['BE'] = new Array('LEB','Beta','','302','1993');
series['UN'] = new Array('2ED','Unlimited','','302','1993');
series['RV'] = new Array('3ED','Revised','3e Edition','306','1994');
series['3B'] = new Array('3ED','3rd Edition (Black Border)','3e Edition (Bords noirs)','306','1994');
series['3W'] = new Array('3ED','3rd Edition (White Border)','3e Edition (Bords blancs)','306','1994');
series['4E'] = new Array('4ED','4th Edition','4e Edition','378','1995');
series['5E'] = new Array('5ED','5th Edition','5e Edition','449','1997');
series['6E'] = new Array('6ED','6th Edition','6e Edition','350','1999');
series['7E'] = new Array('7ED','7th Edition','7e Edition','350','2001');
series['8E'] = new Array('8ED','8th Edition','8e Edition','357','2003');
series['9E'] = new Array('9ED','9th Edition','9e Edition','359','2005');
series['1E'] = new Array('10E','10th Edition','10e Edition','383','2007');
series['M1'] = new Array('M10','Magic 2010','Edition 2010','249','2009');
series['M2'] = new Array('M11','Magic 2011','Edition 2011','249','2010');
series['M3'] = new Array('M12','Magic 2012','Edition 2012','249','2011');
series['M4'] = new Array('M13','Magic 2013','Edition 2013','249','2012');
series['M5'] = new Array('M14','Magic 2014','Edition 2014','249','2013');
series['M6'] = new Array('M15','Magic 2015','Edition 2015','269','2014');
series['OR'] = new Array('ORI','Magic Origins','Magic Origins','272','2015');
series['M9'] = new Array('M19','Magic 2019','Edition 2019','279','2018');
series['M0'] = new Array('M20','Magic 2020','Edition 2020','280','2019');
var bases = new Array('AL','BE','UN','RV','3B','3W','4E','5E','6E','7E','8E','9E','1E','M1','M2','M3','M4','M5','M6','OR','M9','M0');

// Extensions
// No Block
series['AN'] = new Array('ARN','Arabian Nights','','92','1993');
series['AQ'] = new Array('ATQ','Antiquities','','100','1994');
series['LE'] = new Array('LEG','Legends','','310','1994');
series['DK'] = new Array('DRK','The Dark','','119','1994');
series['FE'] = new Array('FEM','Fallen Empires','','187','1994');
series['HM'] = new Array('HML','Homelands','','140','1995');
series['DO'] = new Array('DOM','Dominaria','Dominaria','280','2018');
series['TE'] = new Array('ELD','Throne of Eldraine','Le Trône d\'Eldraine','269','2019');
series['TB'] = new Array('THB','Theros Beyond Death','Theros par-delà la mort','254','2020');
series['IK'] = new Array('IKO','Ikoria: Lair of Behemoths','Ikoria : La Terre des Béhémoths','274','2020');
var horsbloc = new Array('Extensions Hors Bloc','AN','AQ','LE','DK','FE','HM','TE','TB','IK');
// Ice Age Block
series['IA'] = new Array('ICE','Ice Age','Ere Glaciaire','383','1995');
series['AC'] = new Array('ALL','Alliances','Alliances','199','1996');
series['CS'] = new Array('CSP','Coldsnap','Souffle Glaciaire','155','2006');
var iceage = new Array('Bloc Ere Glaciaire','IA','AC','CS');
// Mirage Block
series['MR'] = new Array('MIR','Mirage','Mirage','350','1996');
series['VS'] = new Array('VIS','Visions','Visions','167','1997');
series['WL'] = new Array('WTH','Weatherlight','Aquilon','167','1997');
var mirage = new Array('Bloc Mirage','MR','VS','WL');
// Tempest Block
series['TP'] = new Array('TMP','Tempest','Tempête','350','1997');
series['SH'] = new Array('STH','Stronghold','Forteresse','143','1998');
series['EX'] = new Array('EXO','Exodus','Exode','143','1998');
var tempest = new Array('Bloc Tempête','TP','SH','EX');
// Urza Block
series['US'] = new Array('USG','Urza\'s Saga','L\'Epopée d\'Urza','350','1998');
series['UL'] = new Array('ULG','Urza\'s Legacy','L\'Héritage d\'Urza','143','1999');
series['UD'] = new Array('UDS','Urza\'s Destiny','La Destinée d\'Urza','143','1999');
var urza = new Array('Bloc Urza','US','UL','UD');
// Masques Block
series['MM'] = new Array('MMQ','Mercadian Masques','Les Masques de Mercadia','350','1999');
series['NE'] = new Array('NEM','Nemesis','Némésis','143','2000');
series['PY'] = new Array('PCY','Prophecy','Prophétie','143','2000');
var mercadia = new Array('Bloc Mercadia','MM','NE','PY');
// Invasion Block
series['IV'] = new Array('INV','Invasion','Invasion','350','2000');
series['PS'] = new Array('PLS','Planeshift','Planeshift','143','2000');
series['AP'] = new Array('APC','Apocalypse','Apocalypse','143','2000');
var invasion = new Array('Bloc Invasion','IV','PS','AP');
// Odyssey Block
series['OD'] = new Array('ODY','Odyssey','Odysée','350','2001');
series['TO'] = new Array('TOR','Torment','Tourment','143','2002');
series['JU'] = new Array('JUD','Judgment','Jugement','143','2002');
var odyssey = new Array('Bloc Odyssey','OD','TO','JU');
// Onslaught Block
series['ON'] = new Array('ONS','Onslaught','Carnage','350','2002');
series['LG'] = new Array('LGN','Legions','Légions','145','2003');
series['SC'] = new Array('SCG','Scourge','Fléau','143','2003');
var carnage = new Array('Bloc Carnage','ON','LG','SC');
// Mirrodin Block
series['MD'] = new Array('MRD','Mirrodin','Mirrodin','306','2003');
series['DS'] = new Array('DST','Darksteel','Sombracier','165','2004');
series['FD'] = new Array('5DN','Fifth Dawn','La Cinquième Aube','165','2004');
var mirrodin = new Array('Bloc Mirrodin','MD','DS','FD');
// Kamigawa Block
series['CK'] = new Array('CHK','Champions of Kamigawa','Guerriers de Kamigawa','306','2004');
series['BK'] = new Array('BOK','Betrayers of Kamigawa','Traîtres de Kamigawa','165','2005');
series['SK'] = new Array('SOK','Saviors of Kamigawa','Libérateurs de Kamigawa','165','2005');
var kamigawa = new Array('Bloc Kamigawa','CK','BK','SK');
// Ravnica Block
series['RA'] = new Array('RAV','Ravnica: City of Guilds','Ravnica : La Cité des Guildes','306','2005');
series['GP'] = new Array('GPT','Guildpact','Le Pacte des Guildes','165','2006');
series['DI'] = new Array('DIS','Dissension','Discorde','180','2006');
var ravnica = new Array('Bloc Ravnica','RA','GP','DI');
// Time Spiral Block
series['TS'] = new Array('TSP','Time Spiral','Spirale Temporelle','301','2006');
series['TD'] = new Array('TSB','Time Spiral','Spirale Temporelle','121','2006');
series['PC'] = new Array('PLC','Planar Chaos','Chaos Planaire','165','2007');
series['FS'] = new Array('FUT','Future Sight','Visions De L Avenir','180','2007');
var spirale = new Array('Bloc Spirale Temporelle','TS','TD','PC','FS');
// Lorwyn Block
series['LW'] = new Array('LRW','Lorwyn','Lorwyn','301','2007');
series['MT'] = new Array('MOR','Morningtide','Lèveciel','150','2008');
var lorwyn = new Array('Bloc Lorwyn','LW','MT');
// Shadowmoor Block
series['SM'] = new Array('SHM','Shadowmoor','Sombrelande','301','2008');
series['ET'] = new Array('EVE','Eventide','Coucheciel','180','2008');
var sombrelande = new Array('Bloc Sombrelande','SM','ET');
// Alara Block
series['SL'] = new Array('ALA','Shards Of Alara','Eclats d\'Alara','249','2008');
series['CF'] = new Array('CFX','Conflux','Conflux','145','2009');
series['AR'] = new Array('ARB','Alara Reborn','La Renaissance d\'Alara','145','2009');
var alara = new Array('Bloc Eclats D Alara','SL','CF','AR');
// Zendikar Block
series['ZK'] = new Array('ZEN','Zendikar','Zendikar','249','2009');
series['WW'] = new Array('WWK','Worldwake','Worldwake','145','2010');
series['RI'] = new Array('ROE','Rise of the Eldrazi','L\'Ascension des Eldrazi','248','2010');
var zendikar = new Array('Bloc Zendikar','ZK','WW','RI');
// Scars Of Mirrodin Block
series['SD'] = new Array('SOM','Scars of Mirrodin','Les Cicatrices de Mirrodin','249','2010');
series['MB'] = new Array('MBS','Mirrodin Besieged','Mirrodin Assiégé','155','2011');
series['NP'] = new Array('NPH','New Phyrexia','La Nouvelle Phyrexia','175','2011');
var cicatrices = new Array('Bloc Cicatrices De Mirrodin','SD','MB','NP');
// Innistrad Block
series['IN'] = new Array('ISD','Innistrad','Innistrad','264','2011');
series['DA'] = new Array('DKA','Dark Ascension','Obscure Ascension','158','2012');
series['YR'] = new Array('AVR','Avacyn Restored','Avacyn Ressucitée','244','2012');
var innistrad = new Array('Bloc Innistrad','IN','DA','YR');
// Return To Ravnica Block
series['RR'] = new Array('RTR','Return To Ravnica','Retour sur Ravnica','274','2012');
series['GC'] = new Array('GTC','Gatecrash','Insurrection','249','2013');
series['DZ'] = new Array('DGM','Dragon\'s Maze','Le Labyrinthe du Dragon','156','2013');
var retourRavnica = new Array('Bloc Retour Sur Ravnica','RR','GC','DZ');
// Theros Block
series['TH'] = new Array('THS','Theros','Théros','249','2013');
series['BG'] = new Array('BNG','Born Of The Gods','Créations Divines','165','2014');
series['JN'] = new Array('JOU','Journey To Nyx','Incursion dans Nyx','165','2014');
var theros = new Array('Bloc Theros','TH','BG','JN');
// Khans of Tarkir Block
series['KT'] = new Array('KTK','Khans of Tarkir','Les Khans de Tarkir','269','2014');
series['FR'] = new Array('FRF','Fate Reforged','Destin Reforgé','185','2015');
series['DT'] = new Array('DTK','Dragons of Tarkir','Les Dragons de Tarkir','264','2015');
var tarkir = new Array('Bloc Khans De Tarkir','KT','FR','DT');
// Battle for Zendikar Block
series['BZ'] = new Array('BFZ','Battle for Zendikar','La Bataille de Zendikar','274','2015');
series['OG'] = new Array('OGW','Oath of the Gatewatch','Le serment des Sentinelles','186','2016');
var battlezen = new Array('Bloc La Bataille De Zendikar','BZ','OG');
// Shadows over Innistrad Block
series['SI'] = new Array('SOI','Shadows over Innistrad','Ténèbres sur Innistrad','297','2016');
series['EM'] = new Array('EMN','Eldritch Moon','La Lune Hermétique','205','2016');
var shadowsinni = new Array('Bloc Ténèbres Sur Innistrad','SI','EM');
// Kaladesh Block
series['KD'] = new Array('KLD','Kaladesh','Kaladesh','264','2016');
series['ER'] = new Array('AER','Aether Revolt','La Révolte éthérique','184','2017');
var kalad = new Array('Bloc Kaladesh','KD','ER');
// Amonkhet Block
series['AK'] = new Array('AKH','Amonkhet','Amonkhet','269','2017');
series['HD'] = new Array('HOU','Hour of Devastation','L\'Age de la Destruction','199','2017');
var amonk = new Array('Bloc Amonkhet','AK','HD');
// Ixalan Block
series['XL'] = new Array('XLN','Ixalan','Ixalan','289','2017');
series['XP'] = new Array('E02','Explorers of Ixalan','Explorers of Ixalan','4x60','2017');
series['RX'] = new Array('RIX','Rivals of Ixalan','Les Combattants d\'Ixalan','196','2018');
var ixa = new Array('Bloc Ixalan','XL','XP','RX');
// Guilds of Ravnica Block
series['GR'] = new Array('GRN','Guilds of Ravnica','Les Guildes de Ravnica','259','2018');
series['RG'] = new Array('RNA','Ravnica Allegiance','L\'Allégeance de Ravnica','259','2019');
series['WS'] = new Array('WAR','War of the Spark','La Guerre des Planeswalkers','264','2019');
var guildrav = new Array('Bloc Guildes de Ravnica','GR','RG','WS');
var extensions = new Array(horsbloc,iceage,mirage,tempest,urza,mercadia,invasion,odyssey,carnage,mirrodin,kamigawa,ravnica,spirale,lorwyn,sombrelande,alara,zendikar,cicatrices,innistrad,retourRavnica,theros,tarkir,battlezen,shadowsinni,kalad,amonk,ixa,guildrav);

// Compilations
series['CH'] = new Array('CHR','Chronicles','','125','1995');
series['RE'] = new Array('RE','','Renaissance','122','1995');
series['AT'] = new Array('ATH','Anthologies','','120','1998');
series['BR'] = new Array('BRB','Battle Royale','','4x40','1999');
series['BT'] = new Array('BTD','Beatdown','','2x61','2000');
series['DM'] = new Array('DKM','Deckmasters','','2x62','2001');
series['PG'] = new Array('PG','','Pégase','600','2006');
series['CT'] = new Array('CST','Coldsnap Theme Decks','','62','2007');
series['AH'] = new Array('ARC','Archenemy','','4x60+20','2010');
series['DP'] = new Array('DPA','Duel of the Planeswalkers','','5x60','2010');
series['SV'] = new Array('PS1','Salvat 2011','','600','2011');
series['MS'] = new Array('MMA','Modern Masters','','229','2013');
series['CY'] = new Array('CNS','Conspiracy','','210','2014');
series['MU'] = new Array('MM2','Modern Masters 2015','','249','2015');
series['MA'] = new Array('EMA','Eternal Masters','','249','2016');
series['TC'] = new Array('CN2','Conspiracy: Take the Crown','','221','2016');
series['MW'] = new Array('MM3','Modern Masters 2017','','249','2017');
series['CA'] = new Array('CMA','Commander Anthology','','400','2017');
series['IM'] = new Array('IMA','Iconic Masters','','249','2017');
series['A2'] = new Array('A25','Masters 25','','249','2018');
series['CB'] = new Array('CM2','Commander Anthology Volume II','','400','2018');
series['UM'] = new Array('UMA','Ultimate Masters','','254','2018');
series['UT'] = new Array('PUM','Ultimate Box Topper','','40','2018');
series['MH'] = new Array('MH1','Modern Horizons','Horizons du Modern','254','2019');
var compilations = new Array('CH','RE','AT','BR','BT','DM','PG','CT','AH','DP','SV','MS','CY','MU','MA','TC','MW','CA','IM','A2','CB','UM','UT','MH');

// Masterpiece Series
series['ZX'] = new Array('EXP','Zendikar Expeditions','','45','2016');
series['KI'] = new Array('MPS','Kaladesh Inventions','','30','2016');
series['AJ'] = new Array('MPH','Amonkhet Invocations','','54','2017');
var masterpieceSeries = new Array('ZX','KI','AJ');

// Duel Decks
series['D3'] = new Array('EVG','Duel Decks: Elves vs. Goblins','','2x60','2007');
series['D5'] = new Array('DD2','Duel Decks: Jace vs. Chandra','','2x60','2008');
series['D1'] = new Array('DDC','Duel Decks: Divine vs. Demonic','','2x60','2009');
series['D4'] = new Array('DDD','Duel Decks: Garruk vs. Liliana','','2x60','2009');
series['D6'] = new Array('DDE','Duel Decks: Phyrexia vs. The Coalition','','2x60','2010');
series['D2'] = new Array('DDF','Duel Decks: Elspeth vs. Tezzeret','','2x60','2010');
series['D7'] = new Array('DDG','Duel Decks: Knights vs. Dragons','','2x60','2011');
series['D8'] = new Array('DDH','Duel Decks: Ajani vs. Nicol Bolas','','2x60','2011');
series['D9'] = new Array('DDI','Duel Decks: Venser vs. Koth','','2x60','2012');
series['DG'] = new Array('DDJ','Duel Decks: Izzet vs. Golgari','','2x60','2012');
series['D0'] = new Array('DDK','Duel Decks: Sorin vs. Tibalt','','2x60','2013');
series['DD'] = new Array('DDL','Duel Decks: Heroes vs. Monsters','','2x60','2013');
series['DB'] = new Array('DDM','Duel Decks: Jace vs. Vraska','','2x60','2014');
series['DH'] = new Array('DDN','Duel Decks: Speed vs. Cunning','','2x60','2014');
series['DJ'] = new Array('DDO','Duel Decks: Elspeth vs. Kiora','','2x60','2015');
series['DC'] = new Array('DDP','Duel Decks: Zendikar vs. Eldrazi','','2x60','2015');
series['DQ'] = new Array('DDQ','Duel Decks: Blessed vs. Cursed','','2x60','2016');
series['DR'] = new Array('DDR','Duel Decks: Nissa vs. Ob Nixilis','','2x60','2016');
series['DL'] = new Array('DDS','Duel Decks: Mind vs. Might','','2x60','2017');
series['DU'] = new Array('DDT','Duel Decks: Merfolk vs. Goblins','','2x60','2017');
series['DV'] = new Array('DDU','Duel Decks: Elves vs. Inventors','','2x60','2018');
var dueldecks = new Array('D3','D5','D1','D4','D6','D2','D7','D8','D9','DG','D0','DD','DB','DH','DJ','DC','DQ','DR','DL','DU','DV');

// From The Vault
series['V1'] = new Array('DRB','From The Vault: Dragons','','15','2008');
series['V2'] = new Array('V09','From The Vault: Exiled','','15','2009');
series['V3'] = new Array('V10','From The Vault: Relics','','15','2010');
series['V4'] = new Array('V11','From The Vault: Legendes','','15','2011');
series['V5'] = new Array('V12','From the Vault: Realms','','15','2012');
series['V6'] = new Array('V13','From the Vault: Twenty','','20','2013');
series['V7'] = new Array('V14','From the Vault: Annihilation','','15','2014');
series['V8'] = new Array('V15','From the Vault: Angels','','15','2015');
series['V9'] = new Array('V16','From the Vault: Lore','','15','2016');
series['VA'] = new Array('V17','From the Vault: Transform','','15','2017');
var fromVault = new Array('V1','V2','V3','V4','V5','V6','V7','V8','V9','VA');

// Premium Decks
series['R2'] = new Array('H09','Premium Deck Series Slivers','','60','2009');
series['R1'] = new Array('PD2','Premium Deck Series Fire & Lightning','','60','2010');
series['R3'] = new Array('PD3','Premium Deck Series Graveborn','','60','2011');

// Event Decks
series['MV'] = new Array('MD1','Modern Event Deck 2014','','60+15','2014');
var premiumDecks = new Array('R2','R1','R3','MV');

// Two-Headed Giant
series['BB'] = new Array('BBD','Battlebond','','254','2018');
var giant = new Array('BB');

// Signature Spellbook
series['SS'] = new Array('SS1','Signature Spellbook: Jace','','9','2018');
series['ST'] = new Array('SS2','Signature Spellbook: Gideon','','9','2019');
var spellbook = new Array('SS','ST');

// Global Series
series['GS'] = new Array('GS1','Global Series: Jiang Yanggu and Mu Yanling','','60','2018');
var globalseries = new Array('GS');

// Non Standard Legal Sets
series['PH'] = new Array('HOP','Planechase','','4x60+40','2009');
series['CD'] = new Array('CMD','Commander','','5x100','2011');
series['PI'] = new Array('PC2','Planechase 2012','','4x60+40','2012');
series['C1'] = new Array('CM1','Commander\'s Arsenal','','28','2012');
series['C3'] = new Array('C13','Commander 2013','','5x100','2013');
series['C4'] = new Array('C14','Commander 2014','','5x100','2014');
series['C5'] = new Array('C15','Commander 2015','','5x100','2015');
series['C6'] = new Array('C16','Commander 2016','','5x100','2016');
series['C7'] = new Array('C17','Commander 2017','','4x100','2017');
series['C8'] = new Array('C18','Commander 2018','','4x100','2018');
series['C9'] = new Array('C19','Commander 2019','','4x100','2019');
series['C0'] = new Array('C20','Commander 2020','','5x100','2020');
var nonStandard = new Array('PH','CD','PI','C1','C3','C4','C5','C6','C7','C8','C9','C0');

// Introductory Sets
series['IS'] = new Array('ITP','Introductory Two-Player Set','','4x30','1996');
series['P1'] = new Array('POR','Portal','Portal','222','1997');
series['P2'] = new Array('P02','Portal Second Age','','165','1998');
series['P3'] = new Array('PTK','Portal Three King Kingdoms','','180','1999');
series['S1'] = new Array('S99','Starter 1999','','173','1999');
series['S2'] = new Array('S00','Starter 2000','','57','2000');
series['WD'] = new Array('W16','Welcome Deck 2016','','16','2016');
series['WE'] = new Array('W17','Welcome Deck 2017','','30','2017');
series['GN'] = new Array('GN2','Game Night 2019','','5x60','2019');
var introductory = new Array('IS','P1','P2','P3','S1','S2','WD','WE','GN');

// Non Legal For Tournament Sets
series['UG'] = new Array('UGL','Unglued','','88','1998');
series['UH'] = new Array('UNH','Unhinged','','141','2004');
series['UB'] = new Array('UST','Unstable','','216','2017');
series['UC'] = new Array('UND','Unsanctioned','','96','2020');
var nonlegal = new Array('UG','UH','UB','UC');

// Online Exclusive Sets
series['ME'] = new Array('MED','Masters Edition','','195','2007');
series['O2'] = new Array('ME2','Masters Edition II','','245','2008');
series['O3'] = new Array('ME3','Masters Edition III','','230','2009');
series['O4'] = new Array('ME4','Masters Edition IV','','269','2011');
var online = new Array('ME','O2','O3','O4');

// Promotionnal Sets
series['HP'] = new Array('HP','Harper Prism','','5','1996');
series['PR'] = new Array('PTC','Prerelease Events','','86','1997');
series['JG'] = new Array('JG','Judge Gift','','93','1998');
series['FM'] = new Array('FNM','Friday Night Magic','','75','2000');
series['RT'] = new Array('REP','Release Events','','45','2003');
series['MP'] = new Array('MPR','Magic Player Rewards','','17','2005');
series['GW'] = new Array('GRC','WPN - Gateway','','84','2006');
series['GX'] = new Array('GPX','Grand Prix','','11','2007');
series['MG'] = new Array('MGD','Magic Game Day','','34','2007');
series['MY'] = new Array('MYD','Mythic Edition','','16','2018');
series['GK'] = new Array('GK1','Guilds of Ravnica Guild Kits','','5x60','2018');
var promo = new Array('HP','PR','JG','FM','RT','MP','GW','GX','MG','MY','GK');
