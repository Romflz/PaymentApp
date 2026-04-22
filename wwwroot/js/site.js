document.addEventListener('DOMContentLoaded', () => {
    const form = document.querySelector('form[action="/process"]');
    if (!form) return;

    const name = form.querySelector('[name="SenderName"]');
    const amount = form.querySelector('[name="Amount"]');
    const iban = form.querySelector('[name="Iban"]');
    const submit = form.querySelector('button[type="submit"]');

    function isValid() {
        const nameOk = name.value.trim() !== '';
        const amountOk = parseFloat(amount.value) > 0;
        const ibanOk = iban.value.trim() !== '';
        return nameOk && amountOk && ibanOk;
    }

    function update() {
        submit.disabled = !isValid();
    }

    [name, amount, iban].forEach(el => el.addEventListener('input', update));
    update();
});
