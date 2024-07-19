document.addEventListener("DOMContentLoaded", function () {
    function searchIngredients() {
        // Get the search query entered by the user
        var searchQuery = document.getElementById("search").value.trim();

        // Get all the ingredient labels
        var ingredientLabels = document.querySelectorAll('.add-recipe-container08');

        // Remove any existing "Add ingredient" message
        var addIngredientMessage = document.querySelector('.add-ingredient-message');
        if (addIngredientMessage) {
            addIngredientMessage.remove();
        }

        // Loop through each ingredient label
        var found = false;
        ingredientLabels.forEach(function (label) {
            // Get the ingredient name inside this label
            var ingredientName = label.querySelector('.add-recipe-text5').textContent.trim().toLowerCase();

            // Check if the ingredient name contains the search query
            if (ingredientName.includes(searchQuery.toLowerCase())) {
                // If it does, show the label by removing the 'hidden' class
                label.classList.remove('hidden');
                found = true;
            } else {
                // If it doesn't, hide the label by adding the 'hidden' class
                label.classList.add('hidden');
            }
        });

        // If no ingredient is found, display the "Add ingredient" message
        if (!found && searchQuery !== "") {
            var message = document.createElement('div');
            message.classList.add('add-ingredient-message');
            message.textContent = "Го нема '" + searchQuery + "' на листата, додади?";
            message.style.fontSize = "20px";
            message.style.textAlign = "center";
            message.style.cursor = "pointer";
            message.onclick = function () {
                // Call the backend to add the ingredient using the original case
                addIngredient(searchQuery);
            };
            document.querySelector('.add-recipe-container07').appendChild(message);
        }
    }

    function addIngredient(name) {
        // Call the backend to add the ingredient
        fetch('/recipe/AddIngredient?name=' + encodeURIComponent(name), {
            method: 'POST'
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Failed to add ingredient');
                }
                console.log(response.json());
                window.location.reload();
            })
            .then(data => {
                // Ingredient added successfully, you can handle the response here if needed
                console.log('Ingredient added successfully:', data);
            })
            .catch(error => {
                // Handle errors here
                console.error('Error adding ingredient:', error);
            });
    }

    // Add event listener to the search input
    document.getElementById("search").addEventListener("keyup", searchIngredients);
});
