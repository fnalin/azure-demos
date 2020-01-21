Invoke-WebRequest -Method Post `
    -Uri 'http://localhost:5000/api/clientes' `
    -Headers @{"Content-Type"="application/json"} `
    -Body   '{
                "nome": "Priscila Mitui",
                "local": {
                    "endereco":"Rua Teste, 479",
                    "bairro": "Teste",
                    "cidade": "Toyohashi",
                    "estado":"Aichi",
                    "pais":"Japão"
                }
            }'

Invoke-WebRequest -Method Post `
    -Uri 'http://localhost:5000/api/clientes' `
    -Headers @{"Content-Type"="application/json"} `
    -Body   '{
                "nome": "Raphael Nalin",
                "local": {
                    "endereco":"Rua ABC, 187",
                    "bairro": "XPTO",
                    "cidade": "São Paulo",
                    "estado":"SP",
                    "pais":"Brasil"
                }
            }'
