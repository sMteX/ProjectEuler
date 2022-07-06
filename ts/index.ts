import { BitArray, createSieveFile } from './problems/helpers'
import chalk from 'chalk'

// createSieveFile(100, 'problems/smallSieve.dat')
(async () => {
  const bitArray = await BitArray.from('problems/smallSieve.dat', 9)
  let x = ''
  for (let i = 0; i < bitArray.length; i++) {
    const isPrime = bitArray.get(i)
    x += `${i !== 0 && i % 8 === 0 ? ' ||  ' : ''}${isPrime ? chalk.green(i) : chalk.red(i)} `
  }
  console.log(x)
})()