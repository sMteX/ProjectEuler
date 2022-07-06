import fs from 'fs'
import { pathExistsSync, readFileSync } from 'fs-extra'
import { padStart } from "lodash"
import { SingleBar, Presets } from 'cli-progress'

export const DEFAULT_SIEVE_SIZE = 2_000_000_000 // ~250 MB
export const DEFAULT_SIEVE_FILE = `problems/sieve_${DEFAULT_SIEVE_SIZE}.dat`

type BitArrayArgs = 
  | { type: 'new', bits: number; defaultValue: boolean }
  | { type: 'existing', buffer: Buffer }

export class BitArray {
  private readonly values: Buffer
  private readonly limit: number

  private constructor(args: BitArrayArgs) {
    if (args.type === 'new') {
      const { bits, defaultValue } = args
      const bytes = Math.ceil(bits / 8)
      this.values = Buffer.alloc(bytes, defaultValue ? 255 : 0)
      this.limit = bytes * 8
    } else {
      const { buffer } = args
      this.values = buffer
      this.limit = Buffer.byteLength(buffer) * 8
    }
  }
  /**
   * Creates an empty initialized bit array
   * @param limit Amount of bits. Will be rounded UP to the nearest multiple of 8
   * @param defaultValue Default value to fill the new bit array. Default is false
   */
  public static async from(limit: number, defaultValue?: boolean): Promise<BitArray>;
  /**
   * Reads a bit array from file
   * @param filename Filename
   * @param limit Amount of bits to read. Will be rounded UP to the nearest multiple of 8
   */
  public static async from(filename: string, limit?: number): Promise<BitArray>;
  public static async from(limitOrFilename: number | string, defaultValueOrLimit?: boolean | number): Promise<BitArray> {
    if (typeof limitOrFilename === 'number') {
      const defaultValue = Boolean(defaultValueOrLimit) ?? false
      return new BitArray({ type: 'new', bits: limitOrFilename, defaultValue })
    }
    if (!limitOrFilename || !pathExistsSync(limitOrFilename)) {
      throw new Error('File doesnt exist')
    }
    const bytesToRead = defaultValueOrLimit ? Math.ceil(defaultValueOrLimit as number / 8) : undefined
    const buffer = await this.readNBytes(limitOrFilename, bytesToRead)
    return new BitArray({ type: 'existing', buffer })
  }

  private static async readNBytes(path: fs.PathLike, n?: number): Promise<Buffer> {
    const chunks: any[] = []
    let stream: fs.ReadStream | null = null
    try {
      if (n) {
        // end is inclusive too
        stream = fs.createReadStream(path, { start: 0, end: n - 1 })
      } else {
        stream = fs.createReadStream(path)
      }
      for await (let chunk of stream) {
        chunks.push(chunk)
      }
    } catch(error) {
      console.log('Error while streaming from file', error)
    } finally {
      stream && stream.close()
    }
    return Buffer.concat(chunks)
  }

  // based on https://stackoverflow.com/a/51067029/6031107
  public get(index: number): boolean {
    if (index >= this.limit) {
      throw new Error(`Need bigger sieve, trying bit ${index}, got ${this.limit} values`)
    }
    // e.g. get(15) => 16th bit
    // 00101101 01001101 1001....
    //                 ^
    // byte = 15/8 = 1 (0-based index)
    // bitInByte = 15 % 8 = 7
    // values[1] =    01001101
    // 128 >> 7 = 1 = 00000001
    // bitwise AND =  00000001 => result !== 0 = true
    // ----------------
    // get(14) => 15th bit
    // 00101101 01001101 1001....
    //                ^
    // byte = 14/8 = 1 (0-based index)
    // bitInByte = 14 % 8 = 6
    // values[1] =    01001101
    // 128 >> 6 = 2 = 00000010
    // bitwise AND =  00000000 => result !== 0 = false

    const byte = Math.floor(index / 8)
    const bitInByte = index % 8
    return (this.values[byte] & (128 >> bitInByte)) !== 0
  }

  public set(index: number, value: boolean) {
    if (index >= this.limit) {
      throw new Error(`Need bigger sieve, trying bit ${index}, got ${this.limit} values`)
    }
    const nByte = Math.floor(index / 8)
    const bitInByte = index % 8
    const valueInt = value ? '1' : '0'
    const byte = [...padStart(this.values[nByte].toString(2), 8, '0')]
    byte[bitInByte] = valueInt
    this.values[nByte] = parseInt(byte.join(''), 2)
  }
  
  public printBit(index: number) {
    if (index >= this.limit) {
      throw new Error(`Need bigger sieve, trying bit ${index}, got ${this.limit} values`)
    }
    const byte = this.values[Math.floor(index / 8)]
    const bitInByte = index % 8
    console.log(padStart(byte.toString(2), 8, '0'))
    console.log(padStart('^', bitInByte + 1, ' '))
  }

  public saveToFile(filename: string) {
    fs.writeFileSync(filename, this.values, { flag: 'w' })
  }

  public get length() {
    return this.limit
  }
}

/**
 * 
 * @param limit Amount of numbers to sieve. Will be rounded UP to the nearest multiple of 8
 * @param fileName 
 */
export const createSieveFile = async (limit = DEFAULT_SIEVE_SIZE, fileName = DEFAULT_SIEVE_FILE) => {
  if (limit % 8 !== 0) {
    limit = Math.ceil(limit / 8) * 8
  }
  const primes = await BitArray.from(limit, true)
  primes.set(0, false)
  primes.set(1, false)
  const sqrt = Math.ceil(Math.sqrt(limit))
  const bar = new SingleBar({
    format: 'Primes | {bar} | {percentage}% || {value}/{total} values removed',
  }, Presets.legacy)
  for (let i = 2; i < sqrt; i++) {
    if (!primes.get(i)) {
      console.log(`${i}/${sqrt} not a prime`)
      continue
    }
    console.log(`${i}/${sqrt} is a prime, removing multiples`)
    bar.start(Math.ceil((limit - 2 * i) / i), 2 * i)
    // primes[i] is prime, remove multiples
    for (let j = 2 * i; j < limit; j += i) {
      bar.increment()
      // ignore number if it's already ruled out
      if (primes.get(j)) {
        primes.set(j, false)
      }
    }
    bar.stop()
  }
  console.log('Saving into file')
  primes.saveToFile(fileName)
}