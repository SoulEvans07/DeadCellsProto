import java.io.*;
import java.util.*;

public class BList{
	public static int startnum = 21300000;

	public static void main(String[] args) {
		try {
			String content = new Scanner(new File("list.txt")).useDelimiter("\\Z").next();
			String[] split = content.split("\r\n");
			
			for(int i = 1; i < split.length; i++){
				String num1 = split[i-1].replace("    ", "").split(": ")[0];
				String num2 = split[i].replace("    ", "").split(": ")[0];
				if(Integer.parseInt(num1) + 2 != Integer.parseInt(num2))
					log(num1 + " - " + num2);
			}
		} catch (FileNotFoundException e) {
			e.printStackTrace();
		}
	}

	public static void log(Object o){
		System.out.println(o);
	}
}