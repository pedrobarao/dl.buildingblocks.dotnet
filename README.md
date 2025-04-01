# devlab.buildingblocks.dotnet

## Como usar

Adicione o repositório do GitHub Packages ao seu `NuGet.config`:

```xml
<configuration>
  <packageSources>
    <add key="github" value="https://nuget.pkg.github.com/pedrobarao/index.json" />
  </packageSources>
</configuration>
```

Adicione o pacote ao seu projeto:

```bash
dotnet add package DevLab.Core --version 1.0.0
```

## Conventional Commits

Este projeto usa Conventional Commits para versionamento automático. Certifique-se de seguir o padrão de commits:

- `feat`: Uma nova funcionalidade
- `fix`: Uma correção de bug
- `docs`: Mudanças na documentação
- `style`: Mudanças que não afetam o significado do código (espaços em branco, formatação, etc)
- `refactor`: Mudanças no código que não corrigem um bug nem adicionam uma funcionalidade
- `test`: Adição ou correção de testes
- `chore`: Mudanças no processo de build ou ferramentas auxiliares

## Changelog

O changelog é gerado automaticamente usando o `semantic-release`. Você pode ver todas as mudanças no arquivo `CHANGELOG.md`.