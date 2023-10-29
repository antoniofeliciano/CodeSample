export default function toCamelCaseWithSpaces(inputString) {
    return inputString
    .replace(/([A-Z])/g, ' $1') // Insere um espaço antes de cada letra maiúscula
    .replace(/^./, function(match) {
      return match.toUpperCase(); // Converte a primeira letra para maiúscula
    });
  }