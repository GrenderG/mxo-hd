
using System;

namespace hds
{
	public class CharacterPack{
		
		int numChars;
		int totalChars;
		
		DynamicArray charData;
		DynamicArray charNames;
		NumericalUtils nu;
		
		public CharacterPack(){
			charData = new DynamicArray();
			charNames = new DynamicArray();
			nu = new NumericalUtils();
			numChars = 0;
			totalChars = 0;
		}
		
		public void setTotalChars(int total){
			this.totalChars = total;
		}
		
		public byte[] getByteContents(){
			DynamicArray response = new DynamicArray();
			byte [] totalCharsH = nu.uint16ToByteArray((UInt16)totalChars,1);
			response.append(totalCharsH);
			response.append(charData.getBytes());
			response.append(charNames.getBytes());
			return response.getBytes();
		}
		
		public void addCharacter(string charName, int charId, int status, int serverId){
			byte[] charNameB = new byte[charName.Length+3];
			byte[] hexSize = nu.uint16ToByteArray((UInt16) (charName.Length+1),1);
			charNameB[0] = hexSize[0];
			charNameB[1] = hexSize[1];
			charNameB[charName.Length+2] = 0x00;
			for (int i = 0;i<charName.Length;i++){
				charNameB[2+i] = (byte)charName[i];
			}
			
			
			byte[] charDataB = new byte[14];
			byte[] charIdB = nu.uint32ToByteArray((UInt32)charId,1);
			charDataB[3] = charIdB[0];
			charDataB[4] = charIdB[1];
			charDataB[5] = charIdB[2];
			charDataB[6] = charIdB[3];
			charDataB[9] = 0x00;
			charDataB[10] = (byte) status;
			charDataB[11] = 0x00;
			charDataB[12] = (byte) serverId;
			
			numChars++; // Lets say we have done one ;)
			
			int offset = 0;
			
			// Use formula to calculate offset here 

			offset = 14 + (14*(totalChars-numChars));
						
			if (numChars==1){
				offset += 0;
			}
			else{
				offset += charNames.getSize();
			}
			
			// End of formula
			
			byte [] offsetH = nu.uint16ToByteArray((UInt16)offset,1);
			charDataB[1] = offsetH[0];
			charDataB[2] = offsetH[1];
			
			// After all calculations, append it to the dynamic arrays
			charData.append(charDataB);
			charNames.append(charNameB);
			
		}
		
		public int getPackLength(){
			// We will count the 2bytes of content size too ;)
			return charNames.getSize() + charData.getSize()+2;
		}

	}
	
	public class WorldsPack{
		
		DynamicArray worlds;
		NumericalUtils nu;
		

		int numWorlds;
		
		public WorldsPack(){
			worlds = new DynamicArray();
			nu = new NumericalUtils();
		}
		
		public byte[] getByteContents(){
			DynamicArray response = new DynamicArray();
			byte [] numWorldsH = nu.uint16ToByteArray((UInt16)numWorlds,1);
			response.append(numWorldsH);
			response.append(worlds.getBytes());
			return response.getBytes();
		}
		
		public int getTotalSize(){
			// We also add the 2 length bytes
			return worlds.getSize()+2;
		}
		
		public void addWorld(string worldName, int worldId, int worldStatus, int worldStyle, int worldPopulation){
			byte []world = new byte[32];
			world[0] = 0x00;
			world[1] = (byte) worldId;
						
			for(int i=0;i<worldName.Length;i++){
				world[3+i] = (byte) worldName[i];
			}
			
					
			world[23] = (byte) worldStatus;
			world[24] = (byte) worldStyle;
			
			
			world[25] = 0xd9;
			world[26] = 0x21;
			world[27] = 0x07;
			world[28] = 0x00;
			world[29] = 0x01;
			world[30] = 0x00;
			
			world[31] = (byte) worldPopulation;
			
			worlds.append(world);
			numWorlds++;
		}
		
		

	}
	
	public class WorldList
	{
		bool playerExist;
		
		//DynamicArray worldListData;
		CharacterPack cp;
		WorldsPack wp;
		
		string username;
		string password;
		int userID;
		
		int timeCreated;
		byte[] publicModulus;
		byte[] privateExponent;
		
		public WorldList(){
			//worldListData = new DynamicArray(1);
			cp = new CharacterPack();
			wp = new WorldsPack();
			playerExist = false;
			privateExponent = new byte[96];
			publicModulus = new byte[96];
		}
		
		public CharacterPack getCharPack(){
			return this.cp;
		}
		
		public WorldsPack getWorldPack(){
			return this.wp;
		}
		
		public void setUsername(string username){
			this.username = username;
		}
		
		public void setPassword(string password){
			this.password = password;
		}
		
		public string getUsername(){
			return username;
		}
		
		public string getPassword(){
			return password;
		}
		
		public bool getExistance (){
			return playerExist;
		}
		
		public void setExistance(bool param){
			this.playerExist = param;
		}
		
		public void setUserID(int param){
			this.userID = param;
		}
		
		public int getUserID(){
			return userID;
		}
		
		public void setPublicModulus(byte[] param){
			this.publicModulus = param;
		}
		
		public byte[] getPublicModulus(){
			return publicModulus;
		}
			
			
		public void setTimeCreated(int param){
			this.timeCreated = param;
		}
		
		public int getTimeCreated(){
			return timeCreated;
		}
		
		public void setPrivateExponent(byte[] param){
			this.privateExponent = param;
		}
		
		public byte[] getPrivateExponent(){
			return privateExponent;
		}
		
	}
}
