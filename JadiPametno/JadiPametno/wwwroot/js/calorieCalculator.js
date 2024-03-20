document.addEventListener("DOMContentLoaded", function () {

    // JavaScript to handle the click event and redirect
    document.getElementById('calculator-link').addEventListener('click', function () {

        const targetElement = document.getElementById('calorie-calculator');
        const targetPosition = targetElement.getBoundingClientRect().top;

        // Scroll to the target element with an offset
        window.scrollTo({
            top: targetPosition - 100, // Adjust this value to set the desired offset
            behavior: 'smooth'
        });
    });

    const genderRadios = document.querySelectorAll('input[name="gender"]');
    const ageInput = document.getElementById('age-input');
    const weightInput = document.getElementById('weight-input');
    const heightInput = document.getElementById('height-input');
    const activityRadios = document.querySelectorAll('input[name="activity"]');
    const calculateButton = document.querySelector('.home-calculate-button');
    const tdeeText = document.getElementById('TDEE');

    const cut025Text = document.getElementById('cut025');
    const cut050Text = document.getElementById('cut050');
    const cut100Text = document.getElementById('cut100');
    const bulk025Text = document.getElementById('bulk025');
    const bulk050Text = document.getElementById('bulk050');
    const bulk100Text = document.getElementById('bulk100');

    // Function to check if all required fields are filled
    function areFieldsFilled() {
        return Array.from(genderRadios).some(radio => radio.checked) &&
            ageInput.value.trim() !== '' &&
            weightInput.value.trim() !== '' &&
            heightInput.value.trim() !== '' &&
            Array.from(activityRadios).some(radio => radio.checked);
    }

    // Function to enable or disable the Calculate button based on field completion
    function toggleCalculateButton() {
        calculateButton.disabled = !areFieldsFilled();
    }


    genderRadios.forEach(input => {
        input.addEventListener('change', toggleCalculateButton);
    });
    ageInput.addEventListener('input', toggleCalculateButton);
    weightInput.addEventListener('input', toggleCalculateButton);
    heightInput.addEventListener('input', toggleCalculateButton);
    activityRadios.forEach(input => {
        input.addEventListener('change', toggleCalculateButton);
    });

    // Function to calculate BMR using Harris-Benedict Equation
    function calculateBMR(gender, age, weight, height) {
        let bmr = 0;
        if (gender === 'Маж') {
            bmr = 88.362 + (13.397 * weight) + (4.799 * height) - (5.677 * age);
        } else if (gender === 'Жена') {
            bmr = 447.593 + (9.247 * weight) + (3.098 * height) - (4.330 * age);
        }
        return bmr;
    }

    // Function to calculate TDEE based on BMR and activity level
    function calculateTDEE(bmr, activityLevel) {
        let tdee = 0;
        switch (activityLevel) {
            case 'Седативно (малку или никаква физичка активност)':
                tdee = bmr * 1.2;
                break;
            case 'Лесно активно (лесна вежба/спорт 1-3 дена неделно)':
                tdee = bmr * 1.375;
                break;
            case 'Умерено активно (умерена вежба/спорт 3-5 дена неделно)':
                tdee = bmr * 1.55;
                break;
            case 'Многу активно (тешка вежба/спорт 6-7 дена неделно)':
                tdee = bmr * 1.725;
                break;
            case 'Екстра активно (многу тешка вежба/спорт или физичка работа)':
                tdee = bmr * 1.9;
                break;
            default:
                tdee = bmr * 1.2;
        }

        tdeeText.textContent = tdee.toFixed(0);

        cut025Text.textContent = (tdee * 0.9).toFixed(0) + 'kcal/ден';
        cut050Text.textContent = (tdee * 0.79).toFixed(0) + 'kcal/ден';
        cut100Text.textContent = (tdee * 0.58).toFixed(0) + 'kcal/ден';

        bulk025Text.textContent = (tdee * 1.1).toFixed(0) + 'kcal/ден';
        bulk050Text.textContent = (tdee * 1.21).toFixed(0) + 'kcal/ден';
        bulk100Text.textContent = (tdee * 1.42).toFixed(0) + 'kcal/ден';

        return tdee;
    }

    function calculateCalories() {
        let gender = '';
        let age = parseInt(ageInput.value);
        let weight = parseFloat(weightInput.value);
        let height = parseFloat(heightInput.value);
        let activityLevel = '';

        genderRadios.forEach(radio => {
            if (radio.checked) {
                gender = radio.nextElementSibling.textContent.trim();
            }
        });

        activityRadios.forEach(radio => {
            if (radio.checked) {
                activityLevel = radio.nextElementSibling.textContent.trim();
            }
        });

        const bmr = calculateBMR(gender, age, weight, height);

        const tdee = calculateTDEE(bmr, activityLevel);

        console.log('Basal Metabolic Rate (BMR):', bmr.toFixed(0), 'calories');
        console.log('Total Daily Energy Expenditure (TDEE):', tdee.toFixed(0), 'calories');
    }

    // Attach event listener to the calculate button
    calculateButton.addEventListener('click', calculateCalories);

    // Initially disable the Calculate button
    toggleCalculateButton();
});
