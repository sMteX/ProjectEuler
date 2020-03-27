using System.IO;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace ProjectEuler.problems
{
    public class Problem59 : Problem
    {
        public void run()
        {
            /*
            Each character on a computer is assigned a unique code and the preferred standard is ASCII 
              (American Standard Code for Information Interchange). For example, uppercase A = 65, asterisk (*) = 42, and lowercase k = 107.

            A modern encryption method is to take a text file, convert the bytes to ASCII, then XOR each byte with a given value, 
              taken from a secret key. The advantage with the XOR function is that using the same encryption key on the cipher text, 
              restores the plain text; for example, 65 XOR 42 = 107, then 107 XOR 42 = 65.

            For unbreakable encryption, the key is the same length as the plain text message, and the key is made up of random bytes. 
              The user would keep the encrypted message and the encryption key in different locations, and without both "halves", 
              it is impossible to decrypt the message.

            Unfortunately, this method is impractical for most users, so the modified method is to use a password as a key. 
              If the password is shorter than the message, which is likely, the key is repeated cyclically throughout the message. 
              The balance for this method is using a sufficiently long password key for security, but short enough to be memorable.

            Your task has been made easy, as the encryption key consists of three lower case characters. 
              Using Problem59_cipher.txt, a file containing the encrypted ASCII codes, and the knowledge that the plain text must contain 
              common English words, decrypt the message and find the sum of the ASCII values in the original text.
            
            
            ====
            - let's try top 10 common english words - the, be, to, of, and, a, in, that, have, i
            - key is 3 lower case characters, repeated to match the message length
            - it's a message, so all decrypted characters must be visible (let's assume space to ~, so 32 - 126, including both)
            - if it's a message, it should have all 10 of the words

            - lowercase are 97 to 122, including both
            */

            string text = File.ReadAllText("problems/Problem59_cipher.txt");
            List<int> list = text.Split(",").AsEnumerable().Select(t => int.Parse(t)).ToList();
            var words = new List<string> { "the", "be", "to", "of", "and", "a", "in", "that", "have", "i"};

            List<int> result = new List<int>();
            int sum = 0;
            bool found = false;
            for(char c1 = 'a'; c1 <= 'z' && !found; c1++) {
                for(char c2 = 'a'; c2 <= 'z' && !found; c2++) {
                    for(char c3 = 'a'; c3 <= 'z'; c3++) {
                        result.Clear();
                        int[] currentKey = new int[]{ c1, c2, c3 };
                        for (int i = 0, ki = 0; i < list.Count; i++, ki++) {
                            ki %= 3;
                            result.Add(list[i] ^ currentKey[ki]);
                        }

                        if (result.Any(c => c < 32 || c > 126)) {
                            continue;
                        }
                        string resultString = string.Join("", result.Select(i => ((char)i).ToString()));
                        if (words.Any(w => !resultString.ToLower().Contains(w))) {
                            continue;
                        }
                        foreach (char c in resultString) {
                            sum += (int)c;
                        }
                        found = true;
                        break;
                    }
                }
            }
            System.Console.WriteLine($"Sum of the ASCII values in the decrypted message: {sum}");
        }
    }
}